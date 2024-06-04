using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProductOrders.API.Data;
using ProductOrders.API.IntegrationEvents;
using ProductOrders.API.Repository;
using ProductOrders.API.Serives;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration["ConnectionStrings:DefaultConnection"],
        npgsqlOptionsAction: psqlOptions =>
        {
            psqlOptions.EnableRetryOnFailure(
                             maxRetryCount: 10,
                        maxRetryDelay: TimeSpan.FromSeconds(30), errorCodesToAdd: null);
        }).UseLoggerFactory(LoggerFactory.Create(builder => builder.AddDebug()));
});

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();


builder.Services.AddMassTransit(configurator =>
{
    configurator.SetKebabCaseEndpointNameFormatter();
    configurator.UsingRabbitMq((context, config) =>
    {
        config.Host(new Uri(builder.Configuration["MessageBroker:Host"]!), h =>
        {
            h.Username(builder.Configuration["MessageBroker:UserName"]!);
            h.Password(builder.Configuration["MessageBroker:Password"]!);
        });

        config.ConfigureEndpoints(context);
    });

});

builder.Services.AddTransient<IEventBus, EventBus>();

var app = builder.Build();

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
