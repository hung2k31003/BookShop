using BookShopV2.Models;
using BookShopV2.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private readonly AppDbContext db;
        private readonly UserManager<AppUser> _userManager;
        public CartViewComponent(AppDbContext context, UserManager<AppUser> userManager)
        {
            db = context;
            _userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            var user = _userManager.GetUserAsync(HttpContext.User);
            if (user.Result == null) { 
                return View("Cart",new List<CartVM>());
            }
            var userId = user.Result!.Id;

            var result = db.Carts.Where(c => c.CustomerId == userId)
                .Join(db.Products, c => c.ProductId, p => p.ProductId, (c, p) =>
                new CartVM()
                {
                    CustomerId = c.CustomerId,
                    ProductId = p.ProductId,
                    Price = p.Price,
                    ProductName = p.ProductName,
                    Quantity = c.Quantity,
                    Image = p.Image,
                });
            if (result.Count() == 0)
            {
                return View("Cart", new List<CartVM>());
            }
            return View("Cart", result);
        }
    }
}
