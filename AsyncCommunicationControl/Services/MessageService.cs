using AsyncCommunicationControl.Data;
using AsyncCommunicationControl.Entities;
using AsyncCommunicationControl.Models;
using AsyncCommunicationControl.Services.Interfaces;

namespace AsyncCommunicationControl.Services;

public class MessageService<TCustomMessage> : IMessageService<TCustomMessage> where TCustomMessage : Message, new()
{
    private readonly MessagesContext<TCustomMessage> _messagesContext;

    public MessageService(MessagesContext<TCustomMessage> messagesContext)
    {
        _messagesContext = messagesContext;
    }

    /// <summary>
    /// Pass RetryPolicy if you want your message to be retried
    /// </summary>
    /// <param name="message"></param>
    /// <param name="retryPolicy"></param>
    /// <returns></returns>
    public async Task<int> UpdateMessageAsync(TCustomMessage message, RetryPolicy retryPolicy = null)
    {
        if (retryPolicy != null)
        {
            message.ExecuteAt = DateTime.UtcNow.Add(retryPolicy.RetryInterval);
            message.MaxRetryAttempts = retryPolicy.MaxRetryAttempts;
        }
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