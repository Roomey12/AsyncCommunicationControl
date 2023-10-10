using AsyncCommunicationControl.Services;
using AsyncCommunicationControl.Services.Interfaces;
using MyInfrastructure.Models;
using MyInfrastructure.RabbitMQ;

namespace MyInfrastructure.AsyncCommunication;

public class AsyncCommunicationProducer : IAsyncCommunicationProducer
{
    private readonly IMessageProducer _messageProducer;
    private readonly IMessageService<MyMessage> _messageService;

    public AsyncCommunicationProducer(IMessageProducer messageProducer, IMessageService<MyMessage> messageService)
    {
        _messageProducer = messageProducer;
        _messageService = messageService;
    }
    
    public async Task SendAndSubmitMessageAsync<TMessageContent>(TMessageContent messageContent, string queue)
    {
        var message = await _messageService.CreateAndSubmitMessageAsync(messageContent, queue);
        _messageProducer.SendMessage(message, queue);
    }
    
    public async Task SendAndSubmitCustomMessageAsync<TMessageContent>(TMessageContent messageContent, MyMessage customMessage, string queue) 
    {
        customMessage.Fill(messageContent, queue);
        await _messageService.SubmitMessageAsync(customMessage);
        _messageProducer.SendMessage(customMessage, queue);
    }
}