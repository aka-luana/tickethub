namespace TicketHub.Domain.Entities;

public class SeatHold
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid TicketTypeId { get; set; }
    public Guid EventId { get; set; }

    public int Quantity { get; set; }

    public SeatHoldStatus Status { get; set; }

    public DateTime ExpiresAt { get; set; }
    
    public SeatHold(Guid userId, Guid ticketTypeId, Guid eventId, int quantity, SeatHoldStatus status)
    {
        UserId = userId;
        TicketTypeId = ticketTypeId;
        EventId = eventId;
        Quantity = quantity;
        Status = status;
        ExpiresAt = DateTime.UtcNow.AddMinutes(10);
    }
}
