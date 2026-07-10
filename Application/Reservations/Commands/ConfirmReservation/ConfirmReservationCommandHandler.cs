using MediatR;
using TicketHub.Application.Common;
using TicketHub.Application.Interfaces;
using TicketHub.Domain.Entities;

namespace TicketHub.Application.Reservations.Commands.ConfirmReservation;

public class ConfirmReservationCommandHandler : IRequestHandler<ConfirmReservationCommand, Result<Order>>
{
    private readonly ISeatHoldRepository _seatHoldRepository;
    private readonly IOrderRepository _orderRepository;

    public ConfirmReservationCommandHandler(ISeatHoldRepository seatHoldRepository,  IOrderRepository orderRepository)
    {
        _seatHoldRepository = seatHoldRepository;
        _orderRepository = orderRepository;
    }

    public async Task<Result<Order>> Handle(ConfirmReservationCommand command,
        CancellationToken cancellationToken)
    {
        // todo: precisa implementar
        
        throw null;
    }
}