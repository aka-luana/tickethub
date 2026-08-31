using MediatR;
using TicketHub.Application.Common;
using TicketHub.Application.DTO;
using TicketHub.Domain.Entities;

namespace TicketHub.Application.Reservations.Queries.GetReservationById;

public record GetReservationByIdQuery(Guid Id) : IRequest<Result<ReservationDTO>>;
