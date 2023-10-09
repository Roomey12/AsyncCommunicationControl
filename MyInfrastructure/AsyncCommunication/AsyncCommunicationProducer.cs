using AsyncCommunicationControl.Services;
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
    
    public async Task SendAndSubmitMessage<TMessageContent>(TMessageContent messageContent, string queue)
    {
        var message = await _messageService.CreateAndSubmitMessageAsync(messageContent);
        _messageProducer.SendMessage(message, queue);
    }
    
    public async Task SendAndSubmitCustomMessage<TMessageContent>(TMessageContent messageContent, MyMessage customMessage, string queue) 
    {
        customMessage.Fill(messageContent);
        await _messageService.SubmitMessageAsync(customMessage);
        _messageProducer.SendMessage(customMessage, queue);
    }
}