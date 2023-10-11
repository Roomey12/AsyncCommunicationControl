using AsyncCommunicationControl.Services;
using AsyncCommunicationControl.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using MyInfrastructure.Models;
using MyInfrastructure.RabbitMQ;

namespace MyInfrastructure.AsyncCommunication;

public class AsyncCommunicationProducer : IAsyncCommunicationProducer
{
    private readonly IMessageProducer _messageProducer;
    private readonly IServiceProvider _serviceProvider;

    public AsyncCommunicationProducer(IMessageProducer messageProducer, IServiceProvider serviceProvider)
    {
        _messageProducer = messageProducer;
        _serviceProvider = serviceProvider;
    }
    
    public async Task SendAndUpdateMessageAsync(MyMessage message)
    {
        using var scope = _serviceProvider.CreateScope();
        var messageService = scope.ServiceProvider.GetRequiredService<IMessageService<MyMessage>>();
        await messageService.UpdateMessageAsync(message);
        _messageProducer.SendMessage(message, message.Queue);
    }
    
    public async Task SendAndSubmitMessageAsync<TMessageContent>(TMessageContent messageContent, string queue)
    {
        using var scope = _serviceProvider.CreateScope();
        var messageService = scope.ServiceProvider.GetRequiredService<IMessageService<MyMessage>>();
        var message = await messageService.CreateAndSubmitMessageAsync(messageContent, queue);
        _messageProducer.SendMessage(message, queue);
    }
    
    public async Task SendAndSubmitCustomMessageAsync<TMessageContent>(TMessageContent messageContent, MyMessage customMessage, string queue) 
    {
        using var scope = _serviceProvider.CreateScope();
        var messageService = scope.ServiceProvider.GetRequiredService<IMessageService<MyMessage>>();
        customMessage.Fill(messageContent, queue);
        await messageService.SubmitMessageAsync(customMessage);
        _messageProducer.SendMessage(customMessage, queue);
    }
}