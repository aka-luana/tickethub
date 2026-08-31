using MediatR;
using TicketHub.Application.Common;
using TicketHub.Application.DTO;
using TicketHub.Application.Interfaces;
using TicketHub.Domain.Entities;

namespace TicketHub.Application.Reservations.Queries.GetReservationById;

public class GetReservationByIdQueryHandler : IRequestHandler<GetReservationByIdQuery, Result<ReservationDTO>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ISeatHoldRepository _seatHoldRepository;
    private ILogger _logger;
    
    public GetReservationByIdQueryHandler(IOrderRepository orderRepository,  ISeatHoldRepository seatHoldRepository)
    {
        _orderRepository = orderRepository;
        _seatHoldRepository = seatHoldRepository;
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        _logger = factory.CreateLogger("GetReservationByIdQueryHandler");
    }

    public async Task<Result<ReservationDTO>> Handle(GetReservationByIdQuery query, CancellationToken cancellationToken)
    {
        var isValidAndNotEmpty = query.Id != Guid.Empty;
        if (!isValidAndNotEmpty)
        {
            return Result<ReservationDTO>.Failure(Error.IdIsEmptyError);
        }

        var orderResult = await _orderRepository.GetByIdAsync(query.Id, cancellationToken);
        if (orderResult == null)
        {
            _logger.LogInformation($"Reservation order with id {query.Id} not found");
            
            var seatHoldResult = await _seatHoldRepository.GetByIdAsync(query.Id, cancellationToken);
            if (seatHoldResult == null)
            {
                _logger.LogInformation($"Reservation seat hold with id {query.Id} not found");
                return Result<ReservationDTO>.Failure(Error.NotFoundError);
            }
            
            return Result<ReservationDTO>.Success(MappingToReservationDTO(seatHoldResult));
        }

        return Result<ReservationDTO>.Success(MappingToReservationDTO(orderResult));
    }

    private ReservationDTO MappingToReservationDTO(SeatHold seatHold)
    {
        return new ReservationDTO
        {
            Id = seatHold.Id,
            UserId = seatHold.UserId,
            EventId = seatHold.EventId,
            TicketTypeId = seatHold.TicketTypeId,
            Quantity = seatHold.Quantity,
            Status = MapStatus(seatHold.Status),
            CreatedAt = null,
            UpdatedAt = null
        };
    }

    private ReservationDTO MappingToReservationDTO(Order order)
    {
        return new ReservationDTO
        {
            Id = order.Id,
            UserId = order.UserId,
            EventId = order.EventId,
            TicketTypeId = order.TicketTypeId,
            Quantity = order.Quantity,
            Status = MapStatus(order.Status),
            CreatedAt = order.CreatedAt,
            UpdatedAt = null
        };
    }

    private static ReservationStatusDTO MapStatus(SeatHoldStatus status) => status switch
    {
        SeatHoldStatus.Active => ReservationStatusDTO.Pending,
        SeatHoldStatus.Expired => ReservationStatusDTO.Expired,
        SeatHoldStatus.Converted => ReservationStatusDTO.Confirmed,
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
    };

    private static ReservationStatusDTO MapStatus(OrderStatus status) => status switch
    {
        OrderStatus.Pending => ReservationStatusDTO.Confirmed,
        OrderStatus.Paid => ReservationStatusDTO.Confirmed,
        OrderStatus.Cancelled => ReservationStatusDTO.Canceled,
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
    };
}