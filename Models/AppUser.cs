using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace BookShopV2.Models;

public partial class AppUser:IdentityUser
{

    public string? FullName { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();

    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
