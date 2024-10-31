using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShopV2.Models;

public partial class Shipment
{
    public int ShipmentId { get; set; }

    public DateTime? ShipmentDate { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Country { get; set; }

    public string? ZipCode { get; set; }

    public string? CustomerId { get; set; }

    public virtual AppUser? Customer { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
