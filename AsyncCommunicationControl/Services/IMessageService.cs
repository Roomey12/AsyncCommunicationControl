using AsyncCommunicationControl.Entities;
using AsyncCommunicationControl.Models;

namespace AsyncCommunicationControl.Services;

public interface IMessageService<TCustomMessage> where TCustomMessage : Message, new()
{
    Task<int> SubmitMessageAsync(TCustomMessage message);

    Task<int> UpdateMessageAsync(TCustomMessage message);

    Task<TCustomMessage> CreateAndSubmitMessageAsync<TMessageContent>(TMessageContent content, string queue, ExecutionStatus status = ExecutionStatus.ToBeExecuted);
}