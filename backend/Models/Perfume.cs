using System;
using System.Collections.Generic;

namespace PerfumeShopAPI.Models;

public partial class Perfume
{
    public int PerfumeId { get; set; }

    public string PerfumeName { get; set; } = null!;

    public string Brand { get; set; } = null!;

    public int CategoryId { get; set; }

    public int SizeMl { get; set; }

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public string? Gender { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
