using AsyncCommunicationControl.Data;
using AsyncCommunicationControl.Entities;

namespace AsyncCommunicationControl.Services;

public class RetryService<TCustomMessage> : IRetryService<TCustomMessage> where TCustomMessage : Message, new()
{
    private readonly MessagesContext<TCustomMessage> _messagesContext;
    public RetryService(MessagesContext<TCustomMessage> messagesContext)
    {
        _messagesContext = messagesContext;
    }
    
    public async Task<int> SubmitRetryMessageAsync(TCustomMessage message, RetryPolicy retryPolicy)
    {
        message.ExecuteAt = DateTime.UtcNow.Add(retryPolicy.RetryInterval);
        _messagesContext.Update(message);
        return await _messagesContext.SaveChangesAsync();
    }
    
    public IQueryable<TCustomMessage> GetRetryMessagesAsync(RetryPolicy retryPolicy, string queue)
    {
        return _messagesContext.Messages.Where(message =>
            retryPolicy.RetryStatuses.Contains(message.Status) && 
            message.Queue.Equals(queue, StringComparison.OrdinalIgnoreCase) &&
            message.ExecuteAt <= DateTime.UtcNow &&
            message.TotalRetryAttempts < message.MaxRetryAttempts);
    }
}