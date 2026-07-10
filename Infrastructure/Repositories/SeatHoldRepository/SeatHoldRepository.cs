using Microsoft.EntityFrameworkCore;
using TicketHub.Application.Interfaces;
using TicketHub.Domain.Entities;
using TicketHub.Infrastructure.Persistence;

namespace TicketHub.Infrastructure.Repositories.SeatHoldRepository;

public class SeatHoldRepository : ISeatHoldRepository
{
    private readonly TicketHubDbContext _context;
    
    public SeatHoldRepository(TicketHubDbContext context)
    {
        _context = context;
    }
    
    public async Task<SeatHold?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context
            .SeatHolds
            .FirstOrDefaultAsync(seatHold => seatHold.Id == id, cancellationToken);
    }

    public async Task<SeatHold> CreateAsync(SeatHold seatHold, CancellationToken cancellationToken)
    {
        await _context.SeatHolds.AddAsync(seatHold, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return seatHold;
    }

    public async Task ExpireAsync(SeatHold seatHold, CancellationToken cancellationToken)
    {
        _context.SeatHolds.Update(seatHold);
        await _context.SaveChangesAsync(cancellationToken);
    }
}