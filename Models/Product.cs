using System;
using System.Collections.Generic;

namespace BookShopV2.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }
    public string? Slug {  get; set; }

    public string? Description { get; set; }

    public int? Price { get; set; }

    public int? Stock { get; set; }

    public int? CategoryId { get; set; }

    public string? Publish { get; set; }

    public string? Author { get; set; }

    public DateOnly? ProductDate { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual Category? Category { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
