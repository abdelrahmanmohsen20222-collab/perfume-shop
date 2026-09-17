namespace PerfumeShopAPI.DTOs;

public class OrderItemResponseDto
{
    public int OrderItemId { get; set; }

    public int PerfumeId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public PerfumeResponseDto? Perfume { get; set; }
}
