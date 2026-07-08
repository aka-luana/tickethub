using MediatR;
using TicketHub.Application.Common;
using TicketHub.Application.Interfaces;
using TicketHub.Domain.Entities;

namespace TicketHub.Application.Events.Queries.GetEvents;

public class GetEventsQueryHandler : IRequestHandler<GetEventsQuery, Result<List<Event>>>
{
    private readonly IEventRepository _eventRepository;
    
    public GetEventsQueryHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<Result<List<Event>>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
    {
        var events = await _eventRepository.GetAllAsync(cancellationToken);
        return Result<List<Event>>.Success(events);
    }
    
}