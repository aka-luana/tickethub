using TicketHub.Domain.Entities;

namespace TicketHub.Application.Interfaces;

public interface IEventRepository
{
    Task<Event?> GetByIdAsync(Guid id);
    Task<List<Event>> GetAllAsync();
    Task<Event?> AddAsync(Event e, CancellationToken cancellationToken);
    Task<Event?> UpdateAsync(Guid id, Event e, CancellationToken cancellationToken);
}