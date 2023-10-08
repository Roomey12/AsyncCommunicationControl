using AsyncCommunicationControl.Data;
using AsyncCommunicationControl.Entities;
using AsyncCommunicationControl.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AsyncCommunicationControl.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAsyncCommunicationControl<T>(this IServiceCollection serviceCollection, string connectionString) 
        where T : Message, new()
    {
        serviceCollection.AddDbContext<MessagesContext<T>>(options =>
        {
            options.UseMySql("Server=localhost;Database=AsyncCommunicationControl;User=root;Password=qwer1234;Port=3306;",
                new MySqlServerVersion(new Version(8, 0, 27)),
                b => b.MigrationsAssembly("MyWebApp"));
        });

        serviceCollection.AddScoped<IMessageService<T>, MessageService<T>>();
        
        return serviceCollection;
    }
}