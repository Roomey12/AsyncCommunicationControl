using Microsoft.Extensions.DependencyInjection;
using MyInfrastructure.AsyncCommunication;
using MyInfrastructure.RabbitMQ;

namespace MyInfrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRabbitMQ(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IMessageProducer, RabbitMQProducer>();
        serviceCollection.AddScoped<IAsyncCommunicationProducer, AsyncCommunicationProducer>();
        return serviceCollection;
    }
}