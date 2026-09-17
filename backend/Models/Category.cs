using System;
using System.Collections.Generic;

namespace PerfumeShopAPI.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Perfume> Perfumes { get; set; } = new List<Perfume>();
}
