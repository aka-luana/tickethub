using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TicketHub.Application.DTO;

public class CreateEventDTO
{
    [Required(ErrorMessage = "Name is Required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Artist is Required")]
    public string Artist { get; set; }

    [Required(ErrorMessage = "City is Required")]
    public string City { get; set; }

    [Required(ErrorMessage = "Date is Required")]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "At least one ticket type is required")]
    [MinLength(1, ErrorMessage = "At least one ticket type is required")]
    public List<AddTicketTypeDTO> TicketTypes { get; set; }
}