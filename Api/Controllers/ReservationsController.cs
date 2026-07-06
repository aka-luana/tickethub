using MediatR;
using Microsoft.AspNetCore.Mvc;
using TicketHub.Application.DTO;
using TicketHub.Application.Reservations.Commands.CancelReservation;
using TicketHub.Application.Reservations.Commands.ConfirmReservation;
using TicketHub.Application.Reservations.Commands.CreateReservation;
using TicketHub.Application.Reservations.Queries.GetReservationById;
using TicketHub.Domain.Entities;

namespace TicketHub.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ReservationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReservationsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Order>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetReservationByIdQuery(id));
        if (!result.IsSuccess)
            return NotFound(result.Error);
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<ActionResult<SeatHold>> CreateReservation([FromBody] CreateSeatHoldDTO value)
    {
        var command = new CreateReservationCommand(value.EventId, value.UserId, value.TicketTypeId, value.Quantity);

        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Value);
    }
    
    [HttpPost("{id:guid}/confirm")]
    public async Task<ActionResult<Order>> ConfirmReservation(Guid id)
    {
        var result = await _mediator.Send(new ConfirmReservationCommand(id));
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<SeatHold>> CancelReservation(Guid id)
    {
        var result = await _mediator.Send(new CancelReservationCommand(id));
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Value);
    }
}