using TicketHub.Domain.Entities;

namespace TicketHub.Application.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Order> CreateAsync(Order order, CancellationToken cancellationToken);
    Task CancelAsync(Order order, CancellationToken cancellationToken);
}