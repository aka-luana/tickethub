using Microsoft.EntityFrameworkCore;
using TicketHub.Application.Interfaces;
using TicketHub.Domain.Entities;
using TicketHub.Infrastructure.Persistence;

namespace TicketHub.Infrastructure.Repositories.EventRepository;

public class EventRepository : IEventRepository
{
    // usado para falar com o bd
    private readonly TicketHubDbContext _context;

    public EventRepository(TicketHubDbContext context)
    {
        _context = context;
    }
    
    public async Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Events
            .Include(e => e.TicketTypes)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<List<Event>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Events
            .Include(e => e.TicketTypes)
            .ToListAsync(cancellationToken);
    }

    public async Task<Event?> AddAsync(Event e, CancellationToken cancellationToken)
    {
        await _context.Events.AddAsync(e, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return e;
    }

    public async Task<Event?> UpdateAsync(Event e, CancellationToken cancellationToken)
    {
        _context.Events.Update(e);
        await _context.SaveChangesAsync(cancellationToken);
        return e;
    }
}