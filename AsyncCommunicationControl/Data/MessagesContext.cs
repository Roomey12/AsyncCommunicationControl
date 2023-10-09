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
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<Message>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedOn = DateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.ModifiedOn = DateTime.UtcNow;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}