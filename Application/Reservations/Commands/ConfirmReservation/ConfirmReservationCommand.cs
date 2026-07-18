using MediatR;
using TicketHub.Application.Common;
using TicketHub.Domain.Entities;

namespace TicketHub.Application.Reservations.Commands.ConfirmReservation;

public record ConfirmReservationCommand(Guid SeatHoldId) : IRequest<Result<Order>>;