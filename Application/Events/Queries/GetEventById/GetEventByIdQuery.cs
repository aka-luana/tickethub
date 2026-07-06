using MediatR;
using TicketHub.Application.Common;
using TicketHub.Domain.Entities;

namespace TicketHub.Application.Events.Queries.GetEventById;

public record GetEventByIdQuery(Guid Id) : IRequest<Result<Event>>;