using MediatR;
using TicketHub.Application.Common;
using TicketHub.Domain.Entities;

namespace TicketHub.Application.Reservations.Commands.CreateReservation;

public record CreateReservationCommand(
    Guid EventId,
    Guid UserId,
    Guid TicketTypeId,
    int Quantity) : IRequest<Result<SeatHold>>;