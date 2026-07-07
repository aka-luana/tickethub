using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TicketHub.Infrastructure.Persistence;

public class TicketHubDbContextFactory : IDesignTimeDbContextFactory<TicketHubDbContext>
{
    public TicketHubDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TicketHubDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=tickethub;Username=postgres;Password=postgres");
        return new TicketHubDbContext(optionsBuilder.Options);
    }
    
}