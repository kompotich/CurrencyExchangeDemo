using Crawler.Database;
using CurrencyExchange.Crawler.Core;
using CurrencyExchange.Crawler.Core.Config;
using CurrencyExchange.Crawler.Database;
using CurrencyExchange.Crawler.Database.Entities;
using Hangfire;
using Hangfire.PostgreSql;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Text;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var builder = WebApplication.CreateBuilder(args);

// Settings
builder.Configuration.AddJsonFile("appsettings.Config.json");
builder.Services.Configure<Config>(builder.Configuration);

// Hangfire
var hangfireConnection = builder.Configuration.GetConnectionString("HangfireConnection");
builder.Services.AddHangfire(x => x.UsePostgreSqlStorage(hangfireConnection, new PostgreSqlStorageOptions()
{
    PrepareSchemaIfNecessary = true
}));
builder.Services.AddHangfireServer();

// RabbitMQ
builder.Services.AddMassTransit(x => x.UsingRabbitMq());

// CurrencyExchange.Settings database
var currencyExchangeConnection = builder.Configuration.GetConnectionString("CurrencyExchangeConnection");
builder.Services.AddDbContext<CurrencyExchangeContext>(
    x =>
    {
        x.UseNpgsql(currencyExchangeConnection);
        x.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    },
    ServiceLifetime.Scoped);

//
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Repositories
builder.Services.AddScoped<ISettingRepository, SettingRepository>();

// Crawler.Core services
builder.Services.AddScoped<ICrawlerService, CbrCrawlerService>();
builder.Services.AddScoped<ICrawlerJobService, CbrCrawlerJobService>();

//
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

app.UseHangfireDashboard();

// Run jobs
app.Services.CreateScope().ServiceProvider.GetRequiredService<ICrawlerJobService>().RunStartupJobs();

//
app.Run();