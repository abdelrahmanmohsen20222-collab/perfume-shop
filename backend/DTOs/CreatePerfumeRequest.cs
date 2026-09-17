using System.ComponentModel.DataAnnotations;

namespace PerfumeShopAPI.DTOs;

public class CreatePerfumeRequest
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string PerfumeName { get; set; } = null!;

    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string Brand { get; set; } = null!;

    [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be valid")]
    public int CategoryId { get; set; }

    [Range(1, int.MaxValue)]
    public int SizeMl { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "StockQuantity cannot be negative")]
    public int StockQuantity { get; set; }

    public string? Gender { get; set; }
}