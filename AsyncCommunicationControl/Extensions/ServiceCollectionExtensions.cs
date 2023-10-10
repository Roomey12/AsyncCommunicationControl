using AsyncCommunicationControl.Data;
using AsyncCommunicationControl.Entities;
using AsyncCommunicationControl.Services;
using AsyncCommunicationControl.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AsyncCommunicationControl.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAsyncCommunicationControl<TCustomMessage>(this IServiceCollection serviceCollection,
                                                                                  string connectionString, 
                                                                                  string migrationAssembly) 
        where TCustomMessage : Message, new()
    {
        ArgumentException.ThrowIfNullOrEmpty(connectionString);
        ArgumentException.ThrowIfNullOrEmpty(migrationAssembly);

        serviceCollection.AddDbContext<MessagesContext<TCustomMessage>>(options =>
        {
            options.UseMySql(connectionString,
                new MySqlServerVersion(new Version(8, 0, 27)),
                b => b.MigrationsAssembly(migrationAssembly));
        });

        serviceCollection.AddScoped<IMessageService<TCustomMessage>, MessageService<TCustomMessage>>();
        serviceCollection.AddScoped<IRetryService<TCustomMessage>, RetryService<TCustomMessage>>();
        return serviceCollection;
    }
}