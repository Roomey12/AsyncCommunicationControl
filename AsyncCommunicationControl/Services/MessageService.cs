using System.Text.Json;
using AsyncCommunicationControl.Data;
using AsyncCommunicationControl.Entities;
using AsyncCommunicationControl.Models;

namespace AsyncCommunicationControl.Services;

public class MessageService<T> : IMessageService<T> where T : Message, new()
{
    private readonly MessagesContext<T> _messagesContext;

    public MessageService(MessagesContext<T> messagesContext)
    {
        _messagesContext = messagesContext;
    }

    public async Task<int> SubmitMessageAsync(Message message)
    {
        _messagesContext.Add(message);
        return await _messagesContext.SaveChangesAsync();
    }
    
    public async Task<int> SubmitAndCreateMessageAsync<T1>(T1 content, ExecutionStatus status = ExecutionStatus.ToBeExecuted)
    {
        var jsonContent = JsonSerializer.Serialize(content);
        var message = new T
        {
            Content = jsonContent,
            Status = status
        };

        return await SubmitMessageAsync(message);
    }
}