using TicketHub.Application.Interfaces;
using TicketHub.Domain.Entities;

namespace TicketHub.Infrastructure.Repositories.EventRepository;

public class EventRepository : IEventRepository
{
    public Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<List<Event>> GetAllAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Event?> AddAsync(Event e, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Event?> UpdateAsync(Event e, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}