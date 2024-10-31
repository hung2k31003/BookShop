using System;
using System.Collections.Generic;

namespace BookShopV2.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? Name { get; set; }
    public string? Slug { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
