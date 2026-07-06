using MediatR;
using TicketHub.Application.Common;
using TicketHub.Application.Interfaces;
using TicketHub.Domain.Entities;

namespace TicketHub.Application.Reservations.Commands.ConfirmReservation;

public class ConfirmReservationCommandHandler : IRequestHandler<ConfirmReservationCommand, Result<Order>>
{
    private readonly IReservationRepository _repository;

    public ConfirmReservationCommandHandler(IReservationRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Order>> Handle(ConfirmReservationCommand command,
        CancellationToken cancellationToken)
    {
        // todo: precisa implementar
        
        throw null;
    }
}