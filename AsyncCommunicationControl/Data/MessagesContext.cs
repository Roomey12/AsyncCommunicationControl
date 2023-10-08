using AsyncCommunicationControl.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsyncCommunicationControl.Data;

public class MessagesContext<T> : DbContext where T : Message 
{
    public MessagesContext(DbContextOptions<MessagesContext<T>> options)
        : base(options)
    {

    }
    
    public DbSet<T> Messages { get; set; }
}