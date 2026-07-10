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
        var isValidAndNotEmpty = query.Id != Guid.Empty;
        if (!isValidAndNotEmpty)
        {
            return Result<Order>.Failure(Error.IdIsEmptyError);
        }

        var orderResult = await _repository.GetByIdAsync(query.Id, cancellationToken);
        return orderResult == null ? Result<Order>.Failure(Error.NotFoundError) : Result<Order>.Success(orderResult);
    }
}