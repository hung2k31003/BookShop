using BookShopV2.Models;

namespace BookShopV2.ViewModels
{
    public class CheckoutVM
    {
        public List<CartVM>? CartVMs { get; set; } = new List<CartVM>();
        public Payment? Payment { get; set; } = new Payment(); 
        public Shipment? Shipment { get; set; } = new Shipment(); 
        public AppUser? Customer { get; set; } = new AppUser(); 
    }

}
