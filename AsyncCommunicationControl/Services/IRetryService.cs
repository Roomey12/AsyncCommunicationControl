using AsyncCommunicationControl.Entities;

namespace AsyncCommunicationControl.Services;

public interface IRetryService<TCustomMessage> where TCustomMessage : Message, new()
{
    IQueryable<TCustomMessage> GetRetryMessagesAsync(RetryPolicy retryPolicy, string queue);
}