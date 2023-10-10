using AsyncCommunicationControl.Models;
using AsyncCommunicationControl.Services.Interfaces;
using MyDomain;
using MyInfrastructure;
using MyInfrastructure.Models;

namespace MyConsumer;

public class ProductConsumer : IProductConsumer
{
    private readonly IMessageService<MyMessage> _messageService;
    public ProductConsumer(IMessageService<MyMessage> messageService)
    {
        _messageService = messageService;
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