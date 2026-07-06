using MediatR;
using TicketHub.Application.Common;
using TicketHub.Domain.Entities;

namespace TicketHub.Application.Reservations.Commands.CancelReservation;

public record CancelReservationCommand(Guid ReservationId) : IRequest<Result<SeatHold>>;