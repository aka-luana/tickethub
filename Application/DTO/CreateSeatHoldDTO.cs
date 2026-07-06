using System.ComponentModel.DataAnnotations;

namespace TicketHub.Application.DTO;

public class CreateSeatHoldDTO
{
    [Required]
    public Guid EventId { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    public Guid TicketTypeId { get; set; }
    
    [Required]
    public int Quantity { get; set; }
}