using MediatR;
using TicketHub.Application.Common;
using TicketHub.Application.Interfaces;
using TicketHub.Domain.Entities;

namespace TicketHub.Application.Reservations.Commands.CancelReservation;

public class CancelReservationCommandHandler : IRequestHandler<CancelReservationCommand, Result<SeatHold>>
{
    private readonly IReservationRepository _repository;

    public CancelReservationCommandHandler(IReservationRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<SeatHold>> Handle(CancelReservationCommand command,
        CancellationToken cancellationToken)
    {
        // todo: precisa implementar
        
        throw null;
    }
}