using MediatR;
using TicketHub.Application.Common;
using TicketHub.Application.Interfaces;
using TicketHub.Domain.Entities;

namespace TicketHub.Application.Reservations.Commands.ConfirmReservation;

public class ConfirmReservationCommandHandler : IRequestHandler<ConfirmReservationCommand, Result<Order>>
{
    private readonly ISeatHoldRepository _seatHoldRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IEventRepository _eventRepository;

    public ConfirmReservationCommandHandler(
        ISeatHoldRepository seatHoldRepository,
        IOrderRepository orderRepository,
        IEventRepository eventRepository)
    {
        _seatHoldRepository = seatHoldRepository;
        _orderRepository = orderRepository;
        _eventRepository = eventRepository;
    }

    public async Task<Result<Order>> Handle(ConfirmReservationCommand command, CancellationToken cancellationToken)
    {
        if (command.SeatHoldId == Guid.Empty)
            return Result<Order>.Failure(Error.IdIsEmptyError);

        var seatHold = await _seatHoldRepository.GetByIdAsync(command.SeatHoldId, cancellationToken);
        if (seatHold == null)
            return Result<Order>.Failure(Error.NotFoundError);

        if (seatHold.Status != SeatHoldStatus.Active || seatHold.ExpiresAt < DateTime.UtcNow)
            return Result<Order>.Failure(Error.Invalid);

        var ev = await _eventRepository.GetByIdAsync(seatHold.EventId, cancellationToken);
        var ticketType = ev?.TicketTypes.FirstOrDefault(t => t.Id == seatHold.TicketTypeId);
        if (ev == null || ticketType == null)
            return Result<Order>.Failure(Error.NotFoundError);

        var totalPrice = ticketType.Price * seatHold.Quantity;

        seatHold.Status = SeatHoldStatus.Converted;
        await _seatHoldRepository.UpdateAsync(seatHold, cancellationToken);

        var order = new Order(seatHold.UserId, seatHold.EventId, seatHold.TicketTypeId, seatHold.Quantity, totalPrice);
        var createdOrder = await _orderRepository.CreateAsync(order, cancellationToken);

        return Result<Order>.Success(createdOrder);
    }
}
