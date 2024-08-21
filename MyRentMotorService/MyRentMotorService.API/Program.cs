using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using MyMessageContracts.Contracts;
using MyMessageContracts.Services.Publisher.Interfaces;
using MyMessageContracts.SyncEntities.Consumers.Interfaces;
using MyMessageContracts.SyncEntities.Events;
using MyRentalMotorService.Application.Mappings;
using MyRentalMotorService.Infrastructure.Database.EF.Contexts;
using MyRentalMotorService.Infrastructure.Database.EF.Contexts.Interfaces;
using MyRentalMotorService.Infrastructure.Database.EF.Repositories;
using MyRentalMotorService.Infrastructure.Database.EF.Repositories.Interfaces;
using MyRentalMotorService.Infrastructure.Database.EF.UnitOfWork;
using MyRentalMotorService.Infrastructure.Database.EF.UnitOfWork.Interfaces;
using MyRentalMotorService.Infrastructure.Messaging.RabbitMQ.Consumers;
using MyRentalMotorService.Infrastructure.Repositories;
using MyRentalMotorService.Infrastructure.Services;
using MyRentMotorService.Application.Services;
using MyRentMotorService.Application.Services.Interfaces;
using MyRentMotorService.Domain.Interfaces;
using MyRentMotorService.Domain.RentalAggregate.Entities;
using Polly;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddMassTransit(x =>
{
  x.AddConsumer<CustomerConsumer>();
  x.AddConsumer<MotorcycleConsumer>();

  x.UsingRabbitMq((context, cfg) =>
  {
    cfg.Host("rabbitmq://rabbitmq", h =>
    {
      h.Username("guest");
      h.Password("guest");
    });

    cfg.ReceiveEndpoint("customer_registered_queue", e =>
    {
      e.ConfigureConsumer<CustomerConsumer>(context);
    });

    cfg.ReceiveEndpoint("motorcycle_registered_queue", e =>
    {
      e.ConfigureConsumer<MotorcycleConsumer>(context);
    });

  });
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
  var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
  options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());
builder.Services.AddScoped<IRentalRepository, RentalRepository>();
builder.Services.AddScoped<IEntityRepository<Customer>, EntityRepository<Customer>>();
builder.Services.AddScoped<IEntityRepository<Motorcycle>, EntityRepository<Motorcycle>>();
builder.Services.AddScoped<IEntityRepository<MotorcyclesUnderAnalysis>, EntityRepository<MotorcyclesUnderAnalysis>>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IMotorcycleService, MotorcycleService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ISyncEntityService<CrudEntityEvent<MotorcycleBusEntity>>, MotorcycleCrudBus>();
builder.Services.AddScoped<ISyncEntityService<CrudEntityEvent<CustomerBusEntity>>, CustomerCrudBus>();
builder.Services.AddScoped<IRentalService, RentalService>();
builder.Services.AddScoped<IEventPublisher<CrudEntityEvent<MotorcycleStatusBusEntity>>, EventPublisher<CrudEntityEvent<MotorcycleStatusBusEntity>>>();
builder.Services.AddScoped<IEventPublisher<CrudEntityEvent<CustomerStatusBusEntity>>, EventPublisher<CrudEntityEvent<CustomerStatusBusEntity>>>();

builder.Services.AddAutoMapper(typeof(MappingApiDtosProfile));
builder.Services.AddAutoMapper(typeof(MappingApplicationDtoProfile));

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddControllers()
  .AddJsonOptions(options =>
  {
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
  });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
