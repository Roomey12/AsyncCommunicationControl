using AsyncCommunicationControl.Entities;
using AsyncCommunicationControl.Models;

namespace AsyncCommunicationControl.Services.Interfaces;

public interface IMessageService<TCustomMessage> where TCustomMessage : Message, new()
{
    Task<int> SubmitMessageAsync(TCustomMessage message);

    /// <summary>
    /// Pass RetryPolicy if you want your message to be retried
    /// </summary>
    /// <param name="message"></param>
    /// <param name="retryPolicy"></param>
    /// <returns></returns>
    Task<int> UpdateMessageAsync(TCustomMessage message, RetryPolicy retryPolicy = null);

    Task<TCustomMessage> CreateAndSubmitMessageAsync<TMessageContent>(TMessageContent content, string queue, ExecutionStatus status = ExecutionStatus.ToBeExecuted);
}