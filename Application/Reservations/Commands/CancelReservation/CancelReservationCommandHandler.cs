using MediatR;
using TicketHub.Application.Common;
using TicketHub.Application.Interfaces;
using TicketHub.Domain.Entities;

namespace TicketHub.Application.Reservations.Commands.CancelReservation;

public class CancelReservationCommandHandler : IRequestHandler<CancelReservationCommand, Result<SeatHold>>
{
    private readonly ISeatHoldRepository _repository;

    public CancelReservationCommandHandler(ISeatHoldRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<SeatHold>> Handle(CancelReservationCommand command, CancellationToken cancellationToken)
    {
        var isValidAndNotEmpty = command.SeatHoldId != Guid.Empty;
        if (!isValidAndNotEmpty)
        {
            return Result<SeatHold>.Failure(Error.IdIsEmptyError);
        }

        var seatHoldResult = await _repository.GetByIdAsync(command.SeatHoldId, cancellationToken);
        if (seatHoldResult == null)
            return Result<SeatHold>.Failure(Error.NotFoundError);

        seatHoldResult.Status = SeatHoldStatus.Expired;
        await _repository.UpdateAsync(seatHoldResult, cancellationToken);
        return Result<SeatHold>.Success(seatHoldResult);
    }
}