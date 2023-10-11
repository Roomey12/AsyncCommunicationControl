using AsyncCommunicationControl.Data;
using AsyncCommunicationControl.Entities;
using AsyncCommunicationControl.Models;
using AsyncCommunicationControl.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AsyncCommunicationControl.Services;

public class RetryService<TCustomMessage> : IRetryService<TCustomMessage> where TCustomMessage : Message, new()
{
    private readonly MessagesContext<TCustomMessage> _messagesContext;
    public RetryService(MessagesContext<TCustomMessage> messagesContext)
    {
        _messagesContext = messagesContext;
    }
    
    public async Task<IEnumerable<TCustomMessage>> GetRetryMessagesAsync(IEnumerable<RetryPolicy> retryPolicies)
    {
        return (await Task.WhenAll(retryPolicies.Select(async retryPolicy =>
                {
                    var retryMessages = await GetRetryMessagesAsync(retryPolicy).ToListAsync();
                    retryMessages.ForEach(retryMessage =>
                    {
                        retryMessage.TotalRetryAttempts++;
                        retryMessage.Status = ExecutionStatus.InRetryQueue;
                    });
                    return retryMessages;
                }))
                .ConfigureAwait(false))
            .SelectMany(retryMessages => retryMessages);
    }
    
    private IQueryable<TCustomMessage> GetRetryMessagesAsync(RetryPolicy retryPolicy)
    {
        return _messagesContext.Messages.Where(message =>
            retryPolicy.RetryStatuses.Contains(message.Status) && 
            message.Queue.Equals(retryPolicy.Queue, StringComparison.OrdinalIgnoreCase) &&
            message.ExecuteAt <= DateTime.UtcNow &&
            message.TotalRetryAttempts < message.MaxRetryAttempts);
    }
}