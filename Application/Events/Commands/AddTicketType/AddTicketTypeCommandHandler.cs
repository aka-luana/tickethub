using MediatR;
using TicketHub.Application.Common;
using TicketHub.Application.Interfaces;
using TicketHub.Domain.Entities;

namespace TicketHub.Application.Events.Commands.AddTicketType;

public class AddTicketTypeCommandHandler : IRequestHandler<AddTicketTypeCommand, Result<Event>>
{
    private readonly IEventRepository _eventRepository;

    public AddTicketTypeCommandHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<Result<Event>> Handle(AddTicketTypeCommand request, CancellationToken cancellationToken)
    {
        var existing = await _eventRepository.GetByIdAsync(request.Id);
        if (existing == null)
        {
            return Result<Event>.Failure("Event not found");
        }

        var sameName = existing.TicketTypes
            .Where(t => t.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase))
            .ToList();

        var expectedTier = sameName.Count == 0 ? 1 : sameName.Max(t => t.Tier) + 1;
        if (request.Tier != expectedTier)
        {
            return Result<Event>.Failure(
                $"Invalid tier for '{request.Name}'. Expected Tier {expectedTier}, got Tier {request.Tier}");
        }

        var ticketType = new TicketType(
            Guid.NewGuid(), //TODO: Arrumar depois para passar o id correto do event
            request.Name,
            request.Tier,
            request.Price,
            request.TotalQuantity);

        existing.TicketTypes.Add(ticketType);
        
        var updated = await _eventRepository.UpdateAsync(request.Id, existing, cancellationToken);
        
        return updated == null ? Result<Event>.Failure("Failed to add ticket type to event") : Result<Event>.Success(updated);
    }
}