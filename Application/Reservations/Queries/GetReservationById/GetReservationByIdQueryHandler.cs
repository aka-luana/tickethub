using MediatR;
using TicketHub.Application.Common;
using TicketHub.Application.Interfaces;
using TicketHub.Domain.Entities;

namespace TicketHub.Application.Reservations.Queries.GetReservationById;

public class GetReservationByIdQueryHandler : IRequestHandler<GetReservationByIdQuery, Result<Order>>
{
    private readonly IReservationRepository _repository;

    public GetReservationByIdQueryHandler(IReservationRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Order>> Handle(GetReservationByIdQuery query, CancellationToken cancellationToken)
    {
        var reservation = await _repository.GetByIdAsync(query.Id);

        return reservation == null ? Result<Order>.Failure("Reservation not found") : Result<Order>.Success(reservation);
    }
}