using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfumeShopAPI.Data;
using PerfumeShopAPI.DTOs;
using PerfumeShopAPI.Models;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly PerfumeShopeContext _context;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(PerfumeShopeContext context, ILogger<OrdersController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetOrder()
    {
        var orders = await _context.Orders
            .Include(e => e.Customer)
            .Include(e => e.OrderItems)
            .ThenInclude(e => e.Perfume)
            .ToListAsync();

        var dtos = orders.Select(order => MapToDto(order)).ToList();

        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResponseDto>> GetById(int id)
    {
        var order = await _context.Orders
            .Include(e => e.Customer)
            .Include(e => e.OrderItems)
            .ThenInclude(e => e.Perfume)
            .FirstOrDefaultAsync(e => e.OrderId == id);

        if (order == null)
        {
            return NotFound("not found");
        }

        return Ok(MapToDto(order));
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponseDto>> AddNewOrder(CreateOrderRequest request)
    {
        _logger.LogInformation("Creating order for CustomerId {CustomerId} with {ItemCount} items", request.CustomerId, request.OrderItems.Count);

        var order = new Order
        {
            CustomerId = request.CustomerId,
            OrderDate = DateTime.Now,
            TotalAmount = 0
        };

        foreach (var item in request.OrderItems)
        {
            var perfume = await _context.Perfumes
                .FirstOrDefaultAsync(p => p.PerfumeId == item.PerfumeId);

            if (perfume == null)
            {
                _logger.LogWarning("Order rejected: PerfumeId {PerfumeId} not found", item.PerfumeId);
                return NotFound($"Perfume with id {item.PerfumeId} not found");
            }

            if (perfume.StockQuantity < item.Quantity)
            {
                _logger.LogWarning("Order rejected: not enough stock for PerfumeId {PerfumeId}. Available: {Available}, Requested: {Requested}", perfume.PerfumeId, perfume.StockQuantity, item.Quantity);
                return BadRequest($"Not enough stock for {perfume.PerfumeName}. Available: {perfume.StockQuantity}, requested: {item.Quantity}");
            }

            var orderItem = new OrderItem
            {
                PerfumeId = perfume.PerfumeId,
                Quantity = item.Quantity,
                UnitPrice = perfume.Price
            };

            order.OrderItems.Add(orderItem);
            order.TotalAmount += perfume.Price * item.Quantity;

            perfume.StockQuantity -= item.Quantity;
        }

        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Order {OrderId} created successfully. Total: {Total}", order.OrderId, order.TotalAmount);

        var savedOrder = await _context.Orders
            .Include(e => e.Customer)
            .Include(e => e.OrderItems)
            .ThenInclude(e => e.Perfume)
            .FirstOrDefaultAsync(e => e.OrderId == order.OrderId);

        return CreatedAtAction(
            nameof(GetById),
            new { id = order.OrderId },
            MapToDto(savedOrder!));
    }

    private static OrderResponseDto MapToDto(Order order)
    {
        return new OrderResponseDto
        {
            OrderId = order.OrderId,
            CustomerId = order.CustomerId,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            Customer = order.Customer == null ? null : new CustomerResponseDto
            {
                CustomerId = order.Customer.CustomerId,
                FirstName = order.Customer.FirstName,
                LastName = order.Customer.LastName,
                Email = order.Customer.Email
            },
            OrderItems = order.OrderItems.Select(item => new OrderItemResponseDto
            {
                OrderItemId = item.OrderItemId,
                PerfumeId = item.PerfumeId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                Perfume = item.Perfume == null ? null : new PerfumeResponseDto
                {
                    PerfumeId = item.Perfume.PerfumeId,
                    PerfumeName = item.Perfume.PerfumeName,
                    Brand = item.Perfume.Brand,
                    Price = item.Perfume.Price
                }
            }).ToList()
        };
    }
}