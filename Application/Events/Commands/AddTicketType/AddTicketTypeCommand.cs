using MediatR;
using TicketHub.Application.Common;
using TicketHub.Domain.Entities;

namespace TicketHub.Application.Events.Commands.AddTicketType;

public record AddTicketTypeCommand(
        Guid Id,
        string Name,
        int Tier,
        decimal Price,
        int TotalQuantity,
        int AvailableQuantity
    ) : IRequest<Result<Event>>;