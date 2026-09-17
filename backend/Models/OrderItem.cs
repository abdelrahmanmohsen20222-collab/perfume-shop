using System;
using System.Collections.Generic;

namespace PerfumeShopAPI.Models;

public partial class OrderItem
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public int PerfumeId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Perfume Perfume { get; set; } = null!;
}
