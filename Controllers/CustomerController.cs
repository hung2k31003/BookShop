using BookShopV2.Models;
using BookShopV2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookShopV2.Controllers
{
    public class CustomerController : Controller
    {
        private readonly AppDbContext db;
        private readonly UserManager<AppUser> _userManager;
        public CustomerController(AppDbContext context, UserManager<AppUser> userManager)
        {
            db = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public string GetCurrentUserId()
        {
            var currentUser = _userManager.GetUserAsync(User);
            return currentUser.Result!.Id;
        }

        [Authorize]
        public IActionResult addWishlist(int productId)
        {
            var customerId = GetCurrentUserId();
            if (customerId != null)
            {
                var wishItem = db.Wishlists.Where(w => w.ProductId == productId && w.CustomerId == customerId).FirstOrDefault();
                if (wishItem == null)
                {
                    db.Wishlists.Add(new Wishlist()
                    {
                        CustomerId = customerId,
                        ProductId = productId,
                    });
                    db.SaveChanges();
                }
            }
            return ViewComponent("Wishlist");
        }

        [Authorize]
        public IActionResult removeWishlist(int productId)
        {
            var customerId = GetCurrentUserId();

            var wishItem = db.Wishlists.Where(w => w.ProductId == productId && w.CustomerId == customerId).FirstOrDefault();
            if (wishItem != null)
            {
                db.Remove(wishItem);
                db.SaveChanges();
            }
            return ViewComponent("Wishlist");
        }

        [HttpGet("/mywishlist")]
        [Authorize]
        public IActionResult MyWishlist()
        {
            var customerId = GetCurrentUserId();
            var result = db.Wishlists.Where(w => w.CustomerId!.Equals(customerId))
                .Join(db.Products, w => w.ProductId, p => p.ProductId, (w, p) => new WishlistVM()
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Image = p.Image,
                });
            return View(result);
        }

        [Authorize]
        public IActionResult addCart(int productId, int quantity)
        {
            var customerId = GetCurrentUserId();
            if (customerId != null)
            {
                var cartItem = db.Carts.Where(c => c.ProductId == productId && c.CustomerId == customerId).FirstOrDefault();

                if (cartItem == null)
                {
                    db.Carts.Add(new Cart()
                    {
                        CustomerId = customerId,
                        ProductId = productId,
                        Quantity = quantity,
                    });
                    db.SaveChanges();
                }
                else
                {
                    cartItem.Quantity += quantity;
                    db.SaveChanges();
                }
            }

            return ViewComponent("Cart");
        }

        public IActionResult updateMiniCart()
        {
            return ViewComponent("Cart");
        }

        [Authorize]
        public IActionResult removeCart(int productId)
        {
            var customerId = GetCurrentUserId();

            var cartItem = db.Carts.Where(c => c.ProductId == productId && c.CustomerId == customerId).FirstOrDefault();
            if (cartItem != null)
            {
                db.Remove(cartItem);
                db.SaveChanges();
            }
            return ViewComponent("Cart");
        }

        [HttpGet("/mycart")]
        [Authorize]
        public IActionResult MyCart()
        {
            var customerId = GetCurrentUserId();
            var result = db.Carts.Where(c => c.CustomerId!.Equals(customerId))
                .Join(db.Products, c => c.ProductId, p => p.ProductId, (c, p) => new CartVM()
                {
                    ProductId = p.ProductId,
                    CustomerId = customerId,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Image = p.Image,
                    Quantity = c.Quantity,
                });
            return View(result.ToList());
        }

        public IActionResult updateCart(int productId, int quantity)
        {
            var customerId = GetCurrentUserId();

            var cartItem = db.Carts.Where(c => c.ProductId == productId && c.CustomerId == customerId).FirstOrDefault();
            if (cartItem != null)
            {
                if (quantity == 0)
                {
                    db.Remove(cartItem);
                }
                else if (quantity > 0)
                {
                    cartItem.Quantity = quantity;
                }
                db.SaveChanges();
            }

            var result = db.Carts.Where(c => c.CustomerId == customerId).
                Join(db.Products, c => c.ProductId, p => p.ProductId, (c, p) => new CartVM()
                {
                    ProductId = p.ProductId,
                    CustomerId = customerId,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Image = p.Image,
                    Quantity = c.Quantity,
                }).ToList();
            return PartialView("CartList", result);

        }
    }
}
