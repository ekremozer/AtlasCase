using AtlasCase.Api.BackgroundServices;
using AtlasCase.Api.Services;
using AtlasCase.Data.Core;
using AtlasCase.Data.Core.EntityFramework;
using AtlasCase.Service.Orders;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

AddService(builder.Services);

//var efContext = builder.Services.BuildServiceProvider()?.GetService<IContextFactory>()?.Create();
//efContext?.DbCreator();

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


app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());
app.Run();

void AddService(IServiceCollection services)
{
    services.AddSingleton<IContextFactory, ContextFactory>();
    services.AddSingleton<IOrderService, OrderService>();
    services.AddSingleton(x => new ConnectionFactory { Uri = new Uri(builder.Configuration["AmpqpUrl"]), DispatchConsumersAsync = true });
    services.AddSingleton<RabbitMQClientService>();
    services.AddSingleton<RabbitMQProducer>();
    services.AddHostedService<OrderBackgroundService>();
}