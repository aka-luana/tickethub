using System.ComponentModel.DataAnnotations;
using TicketHub.Domain.Entities;

namespace TicketHub.Application.DTO;

public class UpdateEventStatusDTO
{
    [Required]
    [EnumDataType(typeof(SeatHoldStatus))]
    public SeatHoldStatus Status { get; set; }
}
