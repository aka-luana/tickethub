using MediatR;
using TicketHub.Application.Common;
using TicketHub.Application.Interfaces;
using TicketHub.Domain.Entities;

namespace TicketHub.Application.Events.Commands.CreateEvent;

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, Result<Event>>
{
    private readonly IEventRepository _eventRepository;
    
    public CreateEventCommandHandler(IEventRepository repository)
    {
        _eventRepository = repository;
    }
    
    public async Task<Result<Event>> Handle(CreateEventCommand command, CancellationToken cancellationToken)
    {
        // todo: depois colocar validacao pra nao criar dois eventos iguais - pensar nas chaves compostas

        var newEvent = new Event(
            command.Name,
            command.Artist,
            command.City,
            command.Date
        );

        foreach (var group in command.TicketTypes.GroupBy(t => t.Name.ToLowerInvariant()))
        {
            var tiers = group.Select(t => t.Tier).OrderBy(t => t).ToList();

            for (int i = 0; i < tiers.Count; i++)
            {
                var expected = i + 1;
                if (tiers[i] != expected)
                {
                    return Result<Event>.Failure(
                        $"Tiers for '{group.Key}' must be sequential starting from 1. " +
                        $"Expected Tier {expected}, got Tier {tiers[i]}");
                }
            }
        }

        foreach (var ticketTypeInput in command.TicketTypes)
        {
            var ticketType = new TicketType(
                Guid.NewGuid(), // TODO: Arrumar depois para passar o id de event
                ticketTypeInput.Name,
                ticketTypeInput.Tier,
                ticketTypeInput.Price,
                ticketTypeInput.TotalQuantity
            );
            newEvent.TicketTypes.Add(ticketType);
        }

        var created = await _eventRepository.AddAsync(newEvent, cancellationToken);
        return created == null ? Result<Event>.Failure("Failed to create new event") : Result<Event>.Success(created);
    }
}