using System.ComponentModel.DataAnnotations;

namespace PerfumeShopAPI.DTOs;

public class CreateCategoryRequest
{
    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string CategoryName { get; set; } = null!;

    [StringLength(255)]
    public string? Description { get; set; }
}