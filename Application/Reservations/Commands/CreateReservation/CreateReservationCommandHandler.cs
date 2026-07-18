using MediatR;
using TicketHub.Application.Common;
using TicketHub.Application.Interfaces;
using TicketHub.Domain.Entities;

namespace TicketHub.Application.Reservations.Commands.CreateReservation;

public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, Result<SeatHold>>
{
    private readonly ISeatHoldRepository _seatHoldRepository;
    private readonly IEventRepository _eventRepository;

    public CreateReservationCommandHandler(ISeatHoldRepository repository,  IEventRepository eventRepository)
    {
        _seatHoldRepository = repository;
        _eventRepository = eventRepository;
    }

    public async Task<Result<SeatHold>> Handle(CreateReservationCommand command, CancellationToken cancellationToken)
    {
        var existingEvent = await _eventRepository.GetByIdAsync(command.EventId, cancellationToken);
        if (existingEvent == null)
            return Result<SeatHold>.Failure(Error.NotFoundError);
        
        var ticketType = existingEvent.TicketTypes.FirstOrDefault(t => t.Id == command.TicketTypeId);
        if (ticketType == null)
            return Result<SeatHold>.Failure(Error.NotFoundError);

        // aqui que acontece o race condition - dois requests chegam ao mesmo tempo, ambos passam por essa verificação
        // com AvailableQuantity = 1, ambos acham que têm disponibilidade, e os dois vendem o mesmo ingresso
        if (ticketType.AvailableQuantity < command.Quantity)
            return Result<SeatHold>.Failure("Not enough ticket quantity");
        
        ticketType.AvailableQuantity -= command.Quantity;
        await _eventRepository.UpdateAsync(existingEvent, cancellationToken);
        
        var seatHold = new SeatHold(
            command.UserId, 
            ticketType.Id, 
            existingEvent.Id, 
            command.Quantity, 
            SeatHoldStatus.Active);
        
        var createdSeatHold = await _seatHoldRepository.CreateAsync(seatHold, cancellationToken);
        
        return Result<SeatHold>.Success(createdSeatHold);
    }
}