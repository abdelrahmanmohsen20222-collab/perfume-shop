using System.ComponentModel.DataAnnotations;

namespace PerfumeShopAPI.DTOs;

public class CreateCustomerRequest
{
    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string FirstName { get; set; } = null!;

    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string LastName { get; set; } = null!;

    [EmailAddress]
    public string? Email { get; set; }

    [Phone]
    public string? Phone { get; set; }

    public string? Address { get; set; }
}