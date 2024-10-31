using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShopV2.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? PaymentMethod { get; set; }

    public int? Amount { get; set; }

    public string? CustomerId { get; set; }

    public virtual AppUser? Customer { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
