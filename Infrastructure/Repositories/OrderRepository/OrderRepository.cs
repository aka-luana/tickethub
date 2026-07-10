using Microsoft.EntityFrameworkCore;
using TicketHub.Application.Interfaces;
using TicketHub.Domain.Entities;
using TicketHub.Infrastructure.Persistence;

namespace TicketHub.Infrastructure.Repositories.OrderRepository;

public class OrderRepository : IOrderRepository
{
    private readonly TicketHubDbContext _context;

    public OrderRepository(TicketHubDbContext context)
    {
        _context = context;
    }
    
    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context
            .Orders
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<Order> CreateAsync(Order order, CancellationToken cancellationToken)
    {
        await _context
            .Orders
            .AddAsync(order, cancellationToken);
        await _context
            .SaveChangesAsync(cancellationToken);
        return order;
    }

    public async Task CancelAsync(Order order, CancellationToken cancellationToken)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync(cancellationToken);
    }
}