using AsyncCommunicationControl.Entities;
using AsyncCommunicationControl.Models;

namespace MyInfrastructure;

public class MyMessage : Message
{
    public string? Description { get; set; }

    public MyMessage(string content, string description, ExecutionStatus status = ExecutionStatus.ToBeExecuted) 
        : base(content, status)
    {
        Description = description;
    }

    public MyMessage(string description)
    {
        Description = description;
    }

    public MyMessage()
    {
        
    }
}