using MediatR;
using TicketHub.Application.Common;
using TicketHub.Application.Interfaces;
using TicketHub.Domain.Entities;

namespace TicketHub.Application.Reservations.Commands.CreateReservation;

public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, Result<SeatHold>>
{
    private readonly ISeatHoldRepository _repository;

    public CreateReservationCommandHandler(ISeatHoldRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<SeatHold>> Handle(CreateReservationCommand command,
        CancellationToken cancellationToken)
    {
        // todo: precisa implementar
        
        throw null;
    }
}