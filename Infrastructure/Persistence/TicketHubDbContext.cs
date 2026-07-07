using Microsoft.EntityFrameworkCore;
using TicketHub.Domain.Entities;

namespace TicketHub.Infrastructure.Persistence;

public class TicketHubDbContext : DbContext
{
    public TicketHubDbContext(DbContextOptions<TicketHubDbContext> options) : base(options){}
    
    public DbSet<Event> Events => Set<Event>();
    public DbSet<TicketType> TicketTypes => Set<TicketType>();
    public DbSet<SeatHold> SeatHolds => Set<SeatHold>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TicketType>()
            .Property(t => t.Version)
            .IsRowVersion();

        modelBuilder.Entity<Event>()
            .HasMany(e => e.TicketTypes)
            .WithOne()
            .HasForeignKey(t => t.EventId);
    }
}