namespace TicketHub.Domain.Entities;

public class TicketType
{
    public Guid Id { get; set; }

    public string Name { get; set; } // Pista, VIP, etc.

    public int Tier { get; set; }

    public decimal Price { get; set; }

    public int TotalQuantity { get; set; }
    public int AvailableQuantity { get; set; }

    public TicketType(string name, int tier, decimal price, int totalQuantity, int availableQuantity)
    {
        Name = name;
        Tier = tier;
        Price = price;
        TotalQuantity = totalQuantity;
        AvailableQuantity = availableQuantity;
    }
} 