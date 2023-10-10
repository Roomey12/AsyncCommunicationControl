using AsyncCommunicationControl.Entities;
using AsyncCommunicationControl.Models;

namespace AsyncCommunicationControl.Services.Interfaces;

public interface IRetryService<TCustomMessage> where TCustomMessage : Message, new()
{
    Task<IEnumerable<TCustomMessage>> GetRetryMessagesAsync(IEnumerable<RetryPolicy> retryPolicies);
}