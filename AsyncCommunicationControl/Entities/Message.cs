using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using AsyncCommunicationControl.Models;

namespace AsyncCommunicationControl.Entities;

public class Message
{
    public int Id { get; set; }
    public string StringContent { get; set; }
    public ExecutionStatus Status { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
    
    [NotMapped]
    public object MessageContent { get; set; }

    public Message(object content, ExecutionStatus status = ExecutionStatus.ToBeExecuted)
        : this()
    {
        StringContent = JsonSerializer.Serialize(content);
        MessageContent = content;
        Status = status;
    }

    public Message()
    {
        var utcNow = DateTime.UtcNow;
        CreatedOn = utcNow;
        ModifiedOn = utcNow;
    }

    public void Fill<T>(T stringContent, ExecutionStatus status = ExecutionStatus.ToBeExecuted)
    {
        StringContent = JsonSerializer.Serialize(stringContent);
        Status = status;
    }
}