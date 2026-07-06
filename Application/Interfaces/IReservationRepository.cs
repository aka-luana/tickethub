using TicketHub.Domain.Entities;

namespace TicketHub.Application.Interfaces;

public interface IReservationRepository
{
    Task<Order?> GetByIdAsync(Guid id);
    Task<Order?> CreateAsync(Order r, CancellationToken cancellationToken);
    Task<Order?> CancelAsync(Guid id, Order r, CancellationToken cancellationToken);
    Task<Order?> ConfirmAsync(Guid id, Order r, CancellationToken cancellationToken);
}