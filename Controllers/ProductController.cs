using Microsoft.AspNetCore.Mvc;

namespace BookShopV2.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
