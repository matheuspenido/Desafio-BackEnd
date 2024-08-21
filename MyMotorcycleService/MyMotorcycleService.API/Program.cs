using AutoMapper;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MyMessageContracts.Contracts;
using MyMessageContracts.Services.Publisher.Interfaces;
using MyMessageContracts.SyncEntities.Consumers.Interfaces;
using MyMessageContracts.SyncEntities.Events;
using MyMessageContracts.SyncEntities.Events.Base.Interfaces;
using MyMotorcycleService.Application.Mappings;
using MyMotorcycleService.Application.Services;
using MyMotorcycleService.Application.Services.Interfaces;
using MyMotorcycleService.Domain.Entities;
using MyMotorcycleService.Infrastructure.Database.EF;
using MyMotorcycleService.Infrastructure.Database.EF.Contexts;
using MyMotorcycleService.Infrastructure.Database.EF.Contexts.Interfaces;
using MyMotorcycleService.Infrastructure.Database.EF.Interfaces;
using MyMotorcycleService.Infrastructure.Messaging.RabbitMQ.Consumers;
using MyMotorcycleService.Infrastructure.Services;
using Polly;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddMassTransit(x =>
{
  x.AddConsumer<RentalStatusMotorcycleConsumer>();

  x.UsingRabbitMq((context, cfg) =>
  {
    cfg.Host("rabbitmq://rabbitmq", h =>
    {
      h.Username("guest");
      h.Password("guest");
    });

    cfg.ReceiveEndpoint("motorcycle_status_queue", e =>
    {
      e.ConfigureConsumer<RentalStatusMotorcycleConsumer>(context);
    });
  });
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
  var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
  options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());
builder.Services.AddScoped<IEntityFrameworkRepository<Motorcycle>, EntityFrameworkRepository<Motorcycle>>();
builder.Services.AddScoped<IMotorcycleService, MotorcycleService>();
builder.Services.AddScoped<ISyncEntityService<CrudEntityEvent<MotorcycleStatusBusEntity>>, MotorcycleCrudBus>();
builder.Services.AddScoped<IEventPublisher<IBaseEvent>, EventPublisher<IBaseEvent>>();
builder.Services.AddScoped<IEventPublisher<CrudEntityEvent<MotorcycleBusEntity>>, EventPublisher<CrudEntityEvent<MotorcycleBusEntity>>>();

var mapperConfig = new MapperConfiguration(mc =>
{
  mc.AddProfile(new MappingProfile());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo { Title = "My Motorcycle Service API", Version = "v1" });
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
