using AsyncCommunicationControl;
using AsyncCommunicationControl.Models;
using AsyncCommunicationControl.Services;
using MyDomain;
using MyInfrastructure;

namespace MyConsumer;

public class ProductConsumer : IProductConsumer
{
    private readonly IMessageService<MyMessage> _messageService;
    private readonly RetryPolicy _retryPolicy;
    public ProductConsumer(IMessageService<MyMessage> messageService)
    {
        _messageService = messageService;
        _retryPolicy = new RetryPolicyBuilder()
            .WithExecutionStatus(ExecutionStatus.ExecutedWithErrors)
            .WithMaxRetryAttempts(3)
            .WithRetryInterval(TimeSpan.FromMinutes(1))
            .Build();
    }
    
    public async Task ExecuteAsync(MyMessage message, Product product)
    {
        try
        {
            Console.WriteLine($"Product: {product.Name}|{product.Price}");
            message.Status = ExecutionStatus.SuccessfullyExecuted;
            await _messageService.UpdateMessageAsync(message);
        }
        catch (Exception ex)
        {
            message.Status = ExecutionStatus.ExecutedWithErrors;
            await _messageService.UpdateMessageAsync(message);
        }
    }
}