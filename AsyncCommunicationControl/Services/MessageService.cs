using AsyncCommunicationControl.Data;
using AsyncCommunicationControl.Entities;
using AsyncCommunicationControl.Models;
using Microsoft.EntityFrameworkCore;

namespace AsyncCommunicationControl.Services;

public class MessageService<TCustomMessage> : IMessageService<TCustomMessage> where TCustomMessage : Message, new()
{
    private readonly MessagesContext<TCustomMessage> _messagesContext;

    public MessageService(MessagesContext<TCustomMessage> messagesContext)
    {
        _messagesContext = messagesContext;
    }

    public IQueryable<TCustomMessage> GetMessagesByStatusAndQueue(ExecutionStatus status, string queue)
    {
        return _messagesContext.Messages.Where(message =>
            message.Status == status && 
            message.Queue.Equals(queue, StringComparison.OrdinalIgnoreCase));
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