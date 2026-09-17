using System.ComponentModel.DataAnnotations;

namespace PerfumeShopAPI.DTOs;

public class CreateOrderRequest
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "CustomerId must be a valid positive number")]
    public int CustomerId { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "An order must contain at least one item")]
    public List<CreateOrderItemRequest> OrderItems { get; set; } = new();
}

public class CreateOrderItemRequest
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "PerfumeId must be a valid positive number")]
    public int PerfumeId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; }
}