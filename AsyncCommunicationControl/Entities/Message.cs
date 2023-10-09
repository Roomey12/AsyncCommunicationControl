using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using AsyncCommunicationControl.Models;

namespace AsyncCommunicationControl.Entities;

public class Message
{
    public int Id { get; set; }
    public string Content { get; set; }
    public ExecutionStatus Status { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }

    public Message(object content, ExecutionStatus status = ExecutionStatus.ToBeExecuted)
        : this()
    {
        Content = JsonSerializer.Serialize(content);
        Status = status;
    }

    public Message()
    {
        var utcNow = DateTime.UtcNow;
        CreatedOn = utcNow;
        ModifiedOn = utcNow;
    }

    public void Fill(object content, ExecutionStatus status = ExecutionStatus.ToBeExecuted)
    {
        Content = JsonSerializer.Serialize(content);
        Status = status;
    }
}