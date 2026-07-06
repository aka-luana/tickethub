using MediatR;
using TicketHub.Application.Common;
using TicketHub.Domain.Entities;

namespace TicketHub.Application.Events.Queries.GetEvents;

public record GetEventsQuery : IRequest<Result<List<Event>>>;