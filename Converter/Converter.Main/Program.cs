using Converter.Core.Consumers;
using CurrencyExchange.Converter.Core;
using MassTransit;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        // Services
        services.AddScoped<IConverterService, ConverterService>();

        // RabbitMQ
        services.AddMassTransit(x =>
        {
            x.AddConsumer<CurrencyRubleRateConsumer>();
            x.UsingRabbitMq();
        });

    }).Build();

host.Run();
