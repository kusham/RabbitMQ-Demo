using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProductBilling.API.Data;
using ProductBilling.API.IntegrationEvents.EventHandlers;
using ProductBilling.API.Repository;
using ProductBilling.API.Serives;

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

builder.Services.AddScoped<IBillingRepository, BillingRepository>();
builder.Services.AddScoped<IBillingService, BillingService>();

builder.Services.AddMassTransit(configurator =>
{
    configurator.SetKebabCaseEndpointNameFormatter();

    configurator.AddConsumer<OrderUpdatedEventHandler>();

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
