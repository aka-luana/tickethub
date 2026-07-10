using MediatR;
using TicketHub.Application.Common;
using TicketHub.Application.Interfaces;
using TicketHub.Domain.Entities;

namespace TicketHub.Application.Reservations.Queries.GetReservationById;

public class GetReservationByIdQueryHandler : IRequestHandler<GetReservationByIdQuery, Result<Order>>
{
    private readonly IOrderRepository _repository;

    public GetReservationByIdQueryHandler(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Order>> Handle(GetReservationByIdQuery query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}