using AsyncCommunicationControl.Entities;
using AsyncCommunicationControl.Models;

namespace AsyncCommunicationControl.Services;

public interface IMessageService<T> where T : Message, new()
{
    Task<int> SubmitMessageAsync(Message message);

    Task<int> SubmitAndCreateMessageAsync<T1>(T1 content, ExecutionStatus status = ExecutionStatus.ToBeExecuted);
}