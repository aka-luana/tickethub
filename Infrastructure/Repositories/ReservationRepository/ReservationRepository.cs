using TicketHub.Application.Interfaces;
using TicketHub.Domain.Entities;

namespace TicketHub.Infrastructure.Repositories.ReservationRepository;

public class ReservationRepository : IReservationRepository
{
    public Task<Order?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Order?> CreateAsync(Order r, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Order?> CancelAsync(Guid id, Order r, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Order?> ConfirmAsync(Guid id, Order r, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}