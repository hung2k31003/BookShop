using BookShopV2.Models;
using BookShopV2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet("/myorder")]
        [Authorize]
        public IActionResult MyOrder()
        {
            var customerId = GetCurrentUserId() ;
            var result = from o in db.Orders
                          where o.CustomerId == customerId
                          join s in db.Shipments on o.ShipmentId equals s.ShipmentId
                          join c in db.Users on o.CustomerId equals c.Id
                          select new OrderVM()
                          {
                              OrderId = o.OrderId,
                              OrderDate = o.OrderDate,
                              TotalPrice = o.TotalPrice,
                              CustomerId = c.Id,
                              CustomerName = c.FullName,
                              PhoneNumber = c.PhoneNumber,
                              ShipmentId = s.ShipmentId,
                              State = s.State
                          };
            return View(result);
        }

        [HttpGet("/orderdetail")]
        public IActionResult OrderDetail(int orderId)
        {
            var customerId = GetCurrentUserId();

            var result = db.Orders
                           .Include(o => o.OrderItems)
                           .ThenInclude(oi => oi.Product)
                           .Where(o => o.OrderId == orderId && o.CustomerId == customerId)
                           .Select(o => new OrderDetailVM()
                           {
                               OrderId = o.OrderId,
                               TotalPrice = o.TotalPrice,
                               CustomerName = o.Customer!.FullName,
                               OrderItems = o.OrderItems,
                               PaymentMethod= o.Payment!.PaymentMethod,
                               ShipmentAddress= o.Shipment!.Address,
                           })
                           .FirstOrDefault();

            return View(result);
        }

        [HttpGet("/checkout")]
        [Authorize]
        public IActionResult Checkout()
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
            var kq = new CheckoutVM()
            {
                Customer = db.Users.Where(c => c.Id == customerId).FirstOrDefault(),
                Shipment = null,
                Payment = null,
                CartVMs = result.ToList(),
            };



            return View(kq);
        }

        [HttpPost("/checkout")]
        public IActionResult Checkout(CheckoutVM model)
        {
            var customerId =GetCurrentUserId();
            Payment payment = new Payment()
            {
                PaymentMethod = model.Payment!.PaymentMethod,
                Amount = model.CartVMs!.Sum(c => c.Total),
                CustomerId = customerId,
            };

            db.Add(payment);
            db.SaveChanges();

            Shipment shipment = new Shipment()
            {
                Address = model.Shipment!.Address,
                CustomerId = customerId,
                State = "Chờ xử lý",
            };
            db.Add(shipment);
            db.SaveChanges();

            Order order = new Order()
            {
                OrderDate = DateTime.Now,
                TotalPrice = model.CartVMs!.Sum(c => c.Total),
                CustomerId = customerId,
                PaymentId = payment.PaymentId,
                ShipmentId = shipment.ShipmentId,
            };
            db.Add(order);
            db.SaveChanges();

            foreach (var item in model.CartVMs!)
            {
                var orderItem = new OrderItem()
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    Price = item.Price,
                    Quantity = item.Quantity,
                };
                var product=db.Products.SingleOrDefault(p => p.ProductId == item.ProductId);
                product!.Stock-=item.Quantity;
                db.Add(orderItem);
            }
            db.SaveChanges();

            // Xóa giỏ hàng
            var cartItems = db.Carts.Where(c => c.CustomerId == customerId).ToList();
            db.Carts.RemoveRange(cartItems);
            db.SaveChanges();

            return Redirect("/");
        }
    }
}
