using AsyncCommunicationControl.Extensions;
using MyInfrastructure.Extensions;
using MyInfrastructure.Models;
using MyRetrySystem;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostContext, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: true);
        config.AddJsonFile("appsettings.Development.json", optional: true);
        config.AddEnvironmentVariables();
    })
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;

        services.AddHostedService<RetryWorker>();
        services.AddAsyncCommunicationControl<MyMessage>(
            configuration["Messages:ConnectionString"],
            configuration["Messages:MigrationAssembly"]);
        services.AddRabbitMQ();
    })
    .Build();

host.Run();