using BookShopV2.Models;

namespace BookShopV2.ViewModels
{
    public class OrderVM
    {
        
        public int OrderId { get; set; }

        public DateTime? OrderDate { get; set; }

        public int? TotalPrice { get; set; }

        public string? CustomerId { get; set; }

        public string? CustomerName { get; set; }

        public string? PhoneNumber { get; set; }

        public int ShipmentId { get; set; }
        public string? State { get; set; }
    }

    public class OrderDetailVM()
    {
        public int OrderId { get; set; }

        public int? TotalPrice { get; set; }

        public string? CustomerName { get; set; }
        public string? PaymentMethod { get; set; }
        public string? ShipmentAddress { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    }
}
