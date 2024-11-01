using BookShopV2.Models;
using BookShopV2.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShopV2.ViewComponents
{
    public class WishlistViewComponent : ViewComponent
    {
        private readonly AppDbContext db;
        private readonly UserManager<AppUser> _userManager;
        public WishlistViewComponent(AppDbContext context, UserManager<AppUser> userManager)
        {
            db = context;
            _userManager = userManager;
        }
        public IViewComponentResult Invoke()
        {
            var user = _userManager.GetUserAsync(HttpContext.User);
            if (user.Result == null)
            {
                return View("WishlistVC", new List<WishlistVM>());
            }
            var userId = user.Result!.Id;

            var kq = from w in db.Wishlists
                     join c in db.Users on w.CustomerId equals c.Id
                     join p in db.Products on w.ProductId equals p.ProductId
                     where c.Id == userId
                     select new WishlistVM()
                     {
                         ProductId = p.ProductId,
                         ProductName = p.ProductName,
                         Image = p.Image,
                         Price = p.Price,
                     };
            if (kq.Count() == 0)
            {
                return View("WishlistVC", new List<WishlistVM>());
            }

            return View("WishlistVC", kq);
        }
    }
}
