namespace PerfumeShopAPI.DTOs;

public class PerfumeResponseDto
{
    public int PerfumeId { get; set; }

    public string PerfumeName { get; set; } = null!;

    public string Brand { get; set; } = null!;

    public decimal Price { get; set; }
}