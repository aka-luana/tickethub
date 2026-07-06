using MediatR;
using TicketHub.Application.Common;
using TicketHub.Domain.Entities;

namespace TicketHub.Application.Events.Commands.CreateEvent;

public record TicketTypeInput(
    string Name,
    int Tier,
    decimal Price,
    int TotalQuantity,
    int AvailableQuantity
);

public record CreateEventCommand(
        string Name,
        string Artist,
        string City,
        DateTime Date,
        List<TicketTypeInput> TicketTypes
    ) : IRequest<Result<Event>>;
