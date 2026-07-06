using MediatR;
using Microsoft.AspNetCore.Mvc;
using TicketHub.Application.DTO;
using TicketHub.Application.Events.Commands.AddTicketType;
using TicketHub.Application.Events.Commands.CreateEvent;
using TicketHub.Application.Events.Queries.GetEventById;
using TicketHub.Application.Events.Queries.GetEvents;
using TicketHub.Domain.Entities;

namespace TicketHub.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly IMediator _mediator;

    public EventsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Event>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetEventByIdQuery(id));
        if (!result.IsSuccess)
            return NotFound(result.Error);
        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Event>>> GetAll()
    {
        var result = await _mediator.Send(new GetEventsQuery());
        if (result.Value!.Count == 0)
            return NoContent();
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<ActionResult<Event>> Create([FromBody] CreateEventDTO eventDto)
    {
        var ticketTypes = eventDto.TicketTypes
            .Select(t => new TicketTypeInput(t.Name, t.Tier, t.Price, t.TotalQuantity, t.AvailableQuantity))
            .ToList();

        var command = new CreateEventCommand(
            eventDto.Name,
            eventDto.Artist,
            eventDto.City,
            eventDto.Date,
            ticketTypes
        );

        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
    
    [HttpPost("{id:guid}/ticket-types")]
    public async Task<ActionResult<Event>> AddTicketType(Guid id, [FromBody] AddTicketTypeDTO eventDto)
    {
        var command = new AddTicketTypeCommand(
            id,
            eventDto.Name,
            eventDto.Tier,
            eventDto.Price,
            eventDto.TotalQuantity,
            eventDto.AvailableQuantity
        );
        
        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        
        return Ok(result.Value);
    }
}