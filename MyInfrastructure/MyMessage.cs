using AsyncCommunicationControl.Entities;

namespace MyInfrastructure;

public class MyMessage : Message
{
    public string? Description { get; set; }

    public MyMessage(string description)
    {
        Description = description;
    }
    
    public MyMessage()
    {
        
    }
}