using TicketHub.Domain.Entities;

namespace TicketHub.Application.Interfaces;

public interface ISeatHoldRepository
{
    Task<SeatHold?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<SeatHold> CreateAsync(SeatHold seatHold, CancellationToken cancellationToken);
    Task ExpireAsync(SeatHold seatHold, CancellationToken cancellationToken);
}