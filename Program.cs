using Microsoft.EntityFrameworkCore;
using TicketHub.Application.Interfaces;
using TicketHub.Infrastructure.Persistence;
using TicketHub.Infrastructure.Repositories.EventRepository;
using TicketHub.Infrastructure.Repositories.OrderRepository;
using TicketHub.Infrastructure.Repositories.SeatHoldRepository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddDbContext<TicketHubDbContext>(options =>                                                                                                              
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<ISeatHoldRepository, SeatHoldRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();