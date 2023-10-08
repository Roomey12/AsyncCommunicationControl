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
    
    public async Task<int> SendAndSubmitMessage<T>(T sendMessage, string queue)
    {
        var submitMessageResult = await _messageService.SubmitAndCreateMessageAsync(sendMessage);
        _messageProducer.SendMessage(sendMessage, queue);
        return submitMessageResult;
    }
    
    public async Task<int> SendAndSubmitCustomMessage<T>(T sendMessage, MyMessage submitMessage, string queue) 
    {
        submitMessage.Fill(sendMessage);
        var submitMessageResult = await _messageService.SubmitMessageAsync(submitMessage);
        _messageProducer.SendMessage(sendMessage, queue);
        return submitMessageResult;
    }
}