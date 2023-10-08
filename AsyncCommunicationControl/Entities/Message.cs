using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using AsyncCommunicationControl.Models;

namespace AsyncCommunicationControl.Entities;

public class Message//<T>
{
    public int Id { get; set; }
    public string Content { get; set; }
    public ExecutionStatus Status { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
    
    //[NotMapped]
    //public T Sas { get; set; }

    public Message(string content, ExecutionStatus status)
        : this()
    {
        Content = content;
        Status = status;
    }

    public Message()
    {
        var utcNow = DateTime.UtcNow;
        CreatedOn = utcNow;
        ModifiedOn = utcNow;
    }

    public void Fill<T>(T content, ExecutionStatus status = ExecutionStatus.ToBeExecuted)
    {
        Content = JsonSerializer.Serialize(content);
        Status = status;
    }
}