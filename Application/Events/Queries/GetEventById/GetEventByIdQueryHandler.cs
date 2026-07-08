using MediatR;
using TicketHub.Application.Common;
using TicketHub.Application.Interfaces;
using TicketHub.Domain.Entities;

namespace TicketHub.Application.Events.Queries.GetEventById;

public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, Result<Event>>
{
    private readonly IEventRepository _repository;

    public GetEventByIdQueryHandler(IEventRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Event>> Handle(GetEventByIdQuery query, CancellationToken cancellationToken)
    {
        var e = await _repository.GetByIdAsync(query.Id, cancellationToken);
        
        return e == null ? Result<Event>.Failure("Event not found") : Result<Event>.Success(e);
    }
}