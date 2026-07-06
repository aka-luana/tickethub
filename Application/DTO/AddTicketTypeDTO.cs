using System.ComponentModel.DataAnnotations;

namespace TicketHub.Application.DTO;

public class AddTicketTypeDTO
{
    [MinLength(20)]
    [MaxLength(100)]
    public string Name { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Tier must be at least 1")]
    public int Tier { get; set; }

    public decimal Price { get; set; } = 0;

    [Range(0, 10000, ErrorMessage = "Tickets must be between 0 and 10K")]
    public int TotalQuantity { get; set; }
    [Range(0, 10000, ErrorMessage = "Tickets must be between 0 and 10K")]
    public int AvailableQuantity { get; set; }
}