namespace PerfumeShopAPI.DTOs;

public class OrderResponseDto
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public CustomerResponseDto? Customer { get; set; }

    public List<OrderItemResponseDto> OrderItems { get; set; } = new();
}