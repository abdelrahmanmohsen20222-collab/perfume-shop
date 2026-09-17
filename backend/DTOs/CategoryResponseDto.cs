namespace PerfumeShopAPI.DTOs;

public class CategoryResponseDto
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }
}