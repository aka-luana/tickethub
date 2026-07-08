using TicketHub.Domain.Entities;

namespace TicketHub.Application.Interfaces;

public interface IEventRepository
{
    Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Event>> GetAllAsync(CancellationToken cancellationToken);
    Task<Event?> AddAsync(Event e, CancellationToken cancellationToken);
    Task<Event?> UpdateAsync(Event e, CancellationToken cancellationToken);
}