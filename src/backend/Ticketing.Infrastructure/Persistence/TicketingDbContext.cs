using Microsoft.EntityFrameworkCore;
using Ticketing.Domain.Entities;

namespace Ticketing.Infrastructure.Persistence;

public sealed class TicketingDbContext : DbContext
{
    public TicketingDbContext(DbContextOptions<TicketingDbContext> options) : base(options)
    {
    }

    public DbSet<Ticket> Tickets => Set<Ticket>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.CreatedBy).HasMaxLength(150).IsRequired();
            entity.Property(x => x.AzureDevOpsWorkItemUrl).HasMaxLength(500);
            entity.ComplexProperty(x => x.Title, title =>
            {
                title.Property(t => t.Value).HasColumnName("Title").HasMaxLength(120).IsRequired();
            });
            entity.ComplexProperty(x => x.Description, description =>
            {
                description.Property(t => t.Value).HasColumnName("Description").HasMaxLength(2000).IsRequired();
            });
        });
    }
}
