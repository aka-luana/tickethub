using System.ComponentModel.DataAnnotations;

namespace TicketHub.Application.DTO;

public class AddTicketTypeDTO
{
    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    public string Name { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Tier must be at least 1")]
    public int Tier { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }

    [Range(1, 500000, ErrorMessage = "Total quantity must be between 1 and 500.000")]
    public int TotalQuantity { get; set; }
}
