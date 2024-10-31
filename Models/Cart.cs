using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShopV2.Models;

public partial class Cart
{
    public int? Quantity { get; set; }

    public string? CustomerId { get; set; } 

    public int? ProductId { get; set; } 

    public virtual AppUser? Customer { get; set; } 
    public virtual Product? Product { get; set; }  
}

