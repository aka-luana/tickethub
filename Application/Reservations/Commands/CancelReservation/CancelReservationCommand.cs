using MediatR;
using TicketHub.Application.Common;
using TicketHub.Domain.Entities;

namespace TicketHub.Application.Reservations.Commands.CancelReservation;

public record CancelReservationCommand(Guid SeatHoldId) : IRequest<Result<SeatHold>>;