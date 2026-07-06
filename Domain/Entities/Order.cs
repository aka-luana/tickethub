namespace TicketHub.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public Guid EventId { get; set; }

    public Guid TicketTypeId { get; set; }

    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }

    public OrderStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }
    
    public Order(){}

    public Order(Guid userId, Guid eventId, Guid ticketTypeId, int quantity, decimal totalPrice)
    {
        UserId = userId;
        EventId = eventId;
        TicketTypeId = ticketTypeId;
        Quantity = quantity;
        TotalPrice = totalPrice;
        Status = OrderStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }
}