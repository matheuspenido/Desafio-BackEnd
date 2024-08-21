using Amazon.S3;
using AutoMapper;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MyCustomerService.Application.Mappings;
using MyCustomerService.Application.Services;
using MyCustomerService.Application.Services.Interfaces;
using MyCustomerService.Domain.Entities;
using MyCustomerService.Infrastructure.Database.EF;
using MyCustomerService.Infrastructure.Database.EF.Contexts;
using MyCustomerService.Infrastructure.Database.EF.Contexts.Interfaces;
using MyCustomerService.Infrastructure.Database.EF.Interfaces;
using MyCustomerService.Infrastructure.InfrastructureRepositories;
using MyCustomerService.Infrastructure.InfrastructureRepositories.Interfaces;
using MyCustomerService.Infrastructure.Messaging.RabbitMQ.Consumers;
using MyCustomerService.Infrastructure.Services;
using MyMessageContracts.Contracts;
using MyMessageContracts.Services.Publisher.Interfaces;
using MyMessageContracts.SyncEntities.Consumers.Interfaces;
using MyMessageContracts.SyncEntities.Events;
using MyMessageContracts.SyncEntities.Events.Base.Interfaces;
using Polly;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


DotNetEnv.Env.Load();

// Add services to the container.

builder.Services.AddMassTransit(x =>
{
  x.AddConsumer<RentalStatusCustomerConsumer>();

  x.UsingRabbitMq((context, cfg) =>
  {
    cfg.Host("rabbitmq://rabbitmq", h =>
    {
      h.Username("guest");
      h.Password("guest");
    });

    cfg.ReceiveEndpoint("customer_status_queue", e =>
    {
      e.ConfigureConsumer<RentalStatusCustomerConsumer>(context);
    });
  });
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
  var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
  options.UseNpgsql(connectionString);
});

builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonS3>();

builder.Services.AddScoped<IFileRepository, S3AwsRepository>(provider =>
{
  var s3Client = provider.GetRequiredService<IAmazonS3>();
  var bucketName = builder.Configuration["AWS:BucketName"];

  ArgumentException.ThrowIfNullOrEmpty(bucketName);

  return new S3AwsRepository(s3Client, bucketName);
});

builder.Services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());
builder.Services.AddScoped<IEntityFrameworkRepository<Customer>, EntityFrameworkRepository<Customer>>();
builder.Services.AddScoped<IEntityRepository, CustomerService>();
builder.Services.AddScoped<IDriverLicenseImageService, DriverLicenseImageService>();
builder.Services.AddScoped<IEventPublisher<IBaseEvent>, EventPublisher<IBaseEvent>>();
builder.Services.AddScoped<ISyncEntityService<CrudEntityEvent<CustomerStatusBusEntity>>, CustomerCrudBus>();
builder.Services.AddScoped<IEventPublisher<CrudEntityEvent<CustomerBusEntity>>, EventPublisher<CrudEntityEvent<CustomerBusEntity>>>();

var mapperConfig = new MapperConfiguration(mc =>
{
  mc.AddProfile(new MappingProfile());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddControllers()
  .AddJsonOptions(options =>
  {
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
  });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
  c.OperationFilter<SwaggerFileOperationFilter>();
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
  var services = scope.ServiceProvider;

  var migrationRetryPolicy = Policy
  .Handle<Exception>()
  .WaitAndRetry(5, retryAttempt =>
    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
    (exception, timeSpan, retryCount, context) =>
    {
      Console.WriteLine($"Migration failed. Waiting {timeSpan.TotalSeconds} seconds before next retry. Retry attempt {retryCount}.");
    });

  try
  {
    var context = services.GetRequiredService<AppDbContext>();
    migrationRetryPolicy.Execute(() =>
    {
      context.Database.Migrate();
      Console.WriteLine("Database migration completed successfully.");
    });

  }
  catch (Exception ex)
  {
    Console.WriteLine($"An error ocurred migrating the database: {ex.Message}");
  }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


public class SwaggerFileOperationFilter : IOperationFilter
{
  public void Apply(OpenApiOperation operation, OperationFilterContext context)
  {
    var fileParams = context.MethodInfo.GetParameters()
        .Where(p => p.ParameterType == typeof(IFormFile))
        .ToArray();

    if (fileParams.Any())
    {
      operation.Parameters.Clear();

      operation.Parameters.Add(new OpenApiParameter
      {
        Name = "driverLicense",
        In = ParameterLocation.Path,
        Required = true,
        Schema = new OpenApiSchema
        {
          Type = "string"
        }
      });

      operation.RequestBody = new OpenApiRequestBody
      {
        Content = new Dictionary<string, OpenApiMediaType>
        {
          ["multipart/form-data"] = new OpenApiMediaType
          {
            Schema = new OpenApiSchema
            {
              Type = "object",
              Properties = {
                ["file"] = new OpenApiSchema
                {
                  Type = "string",
                  Format = "binary"
                }
              },
              Required = new HashSet<string> { "file" }
            }
          }
        }
      };
    }
  }
}