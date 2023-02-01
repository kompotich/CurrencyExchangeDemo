using CurrencyExchange.Storage.Core;
using CurrencyExchange.Storage.Core.Cache;
using CurrencyExchange.Storage.Core.Cache.Reports.CurrencyRubleRateReport;
using CurrencyExchange.Storage.Core.Consumers;
using CurrencyExchange.Storage.Core.Reports.CurrencyRubleRateReport;
using CurrencyExchange.Storage.Database.Entities;
using CurrencyExchange.Storage.Database.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CurrencyReferenceConsumer>();
    _ = x.AddConsumer<CurrencyRateConsumer>();
    x.UsingRabbitMq();
});

// Cache
builder.Services.AddStackExchangeRedisCache(x =>
{
    x.Configuration = builder.Configuration.GetConnectionString("CacheConnection");
    x.InstanceName = "CurrencyExchange";
});

// App database
builder.Services.AddDbContext<CurrencyExchangeContext>(
    x => x.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnection")));
// App database repositories
builder.Services.AddScoped<ICurrencyReferenceRepository, CurrencyReferenceRepository>();
builder.Services.AddScoped<ICurrencyRateRepository, CurrencyRateRepository>();
// App database repos wrapper
builder.Services.AddScoped<ICurrencyExchangeRepository, CurrencyExchangeRepository>();

// Data services
builder.Services.AddScoped<ICurrencyReferenceService, CurrencyReferenceService>();
builder.Services.AddScoped<ICurrencyRateService, CurrencyRateService>();
// Report services
builder.Services.AddScoped<ICurrencyRubleRateReportService, CurrencyRubleRateReportService>();
// Cache services
builder.Services.AddSingleton<ICurrencyRubleRateReportCacheService, CurrencyRubleRateReportCacheService>();

// Api
builder.Services.AddControllers();
// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<CacheHostedService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
