using AsyncCommunicationControl.Helpers;
using AsyncCommunicationControl.Models;
using AsyncCommunicationControl.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using MyDomain;
using MyDomain.Models;
using MyInfrastructure.Models;

namespace MyConsumer.Consumer;

public class ProductConsumer : IProductConsumer
{
    private readonly IServiceProvider _serviceProvider;

    public static RetryPolicy RetryPolicy = new RetryPolicyBuilder()
        .WithQueue("products")
        .WithExecutionStatus(ExecutionStatus.ExecutedWithErrors)
        .WithMaxRetryAttempts(3)
        .WithRetryInterval(TimeSpan.FromSeconds(30))
        .Build();
    //private readonly IMessageService<MyMessage> _messageService;
    public ProductConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task ExecuteAsync(MyMessage message, Product product)
    {
        using var scope = _serviceProvider.CreateScope();
        var messageService = scope.ServiceProvider.GetRequiredService<IMessageService<MyMessage>>();
        try
        {
            Console.WriteLine($"Product: {product.Name}|{product.Price}");
            if (product.Price > 10)
            {
                throw new Exception("wrong price");
            }
            message.Status = ExecutionStatus.SuccessfullyExecuted;
            await messageService.UpdateMessageAsync(message);
        }
        catch (Exception ex)
        {
            message.Status = ExecutionStatus.ExecutedWithErrors;
            try
            {
                await messageService.UpdateMessageAsync(message, RetryPolicy);
            }
            catch (Exception ex1)
            {
                Console.WriteLine(ex1.Message);
            }
        }
    }
}