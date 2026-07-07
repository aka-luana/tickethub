namespace TicketHub.Domain.Entities;

public class Event
{
    public Guid Id { get; set; }

    public string Name { get; private set; }
    public string Artist { get; private set; }
    public string City { get; private set; }
    public DateTime Date { get; private set; }

    public List<TicketType> TicketTypes { get; private set; } = new();
    
    public Event()
    {
    }

    public Event(string name, string artist, string city, DateTime date)
    {
        Id = Guid.NewGuid();
        Name = name;
        Artist = artist;
        City = city;
        Date = date;
    }
}
