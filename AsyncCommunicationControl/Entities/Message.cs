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
    public string Queue { get; set; }
    public DateTime ExecuteAt { get; set; }
    public int MaxRetryAttempts { get; set; }
    public int TotalRetryAttempts { get; set; }

    public void Fill(object content, string queue, ExecutionStatus status = ExecutionStatus.ToBeExecuted)
    {
        Content = JsonSerializer.Serialize(content);
        Status = status;
        Queue = queue;
    }
}