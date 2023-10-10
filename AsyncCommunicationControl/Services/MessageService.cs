using AsyncCommunicationControl.Data;
using AsyncCommunicationControl.Entities;
using AsyncCommunicationControl.Models;

namespace AsyncCommunicationControl.Services;

public class MessageService<TCustomMessage> : IMessageService<TCustomMessage> where TCustomMessage : Message, new()
{
    private readonly MessagesContext<TCustomMessage> _messagesContext;

    public MessageService(MessagesContext<TCustomMessage> messagesContext)
    {
        _messagesContext = messagesContext;
    }

    public async Task<int> UpdateMessageAsync(TCustomMessage message)
    {
        _messagesContext.Update(message);
        return await _messagesContext.SaveChangesAsync();
    }

    public async Task<int> SubmitMessageAsync(TCustomMessage message)
    {
        _messagesContext.Add(message);
        return await _messagesContext.SaveChangesAsync();
    }

    public async Task<TCustomMessage> CreateAndSubmitMessageAsync<TMessageContent>(TMessageContent content, string queue, ExecutionStatus status = ExecutionStatus.ToBeExecuted)
    {
        var message = new TCustomMessage();
        message.Fill(content, queue, status);
        await SubmitMessageAsync(message);
        return message;
    }
}