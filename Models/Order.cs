using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShopV2.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public DateTime? OrderDate { get; set; }

    public int? TotalPrice { get; set; }

    public string? CustomerId { get; set; }

    public int? PaymentId { get; set; }

    public int? ShipmentId { get; set; }

    public virtual AppUser? Customer { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Payment? Payment { get; set; }

    public virtual Shipment? Shipment { get; set; }
}
