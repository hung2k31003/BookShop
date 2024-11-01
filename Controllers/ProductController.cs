using BookShopV2.ViewModels;
using BookShopV2.Models;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace BookShopV2.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext db;
        public ProductController(AppDbContext context)
        {
            db = context;
        }

        public IActionResult Index(int? page, int? category, string? searchString, int? sortType)
        {
            int pageNumber = page ?? 1;
            int pageSize = 3;
            var products = db.Products.OrderByDescending(p => p.ProductId).AsQueryable();

            if (category.HasValue)
            {
                products = products.Where(p => p.Category!.CategoryId == category.Value);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.ProductName!.Contains(searchString));
            }

            if (sortType.HasValue)
            {
                if (sortType == 1)
                {
                    products = products.OrderBy(p => p.Price);
                }
                else if (sortType == 2)
                {
                    products = products.OrderByDescending(p => p.Price);
                }
                else if (sortType == 3)
                {
                    products = products.OrderBy(p => p.ProductName);
                }
                else if (sortType == 4)
                {
                    products = products.OrderByDescending(p => p.ProductName);
                }
                else
                {
                    products = products.OrderByDescending(p => p.ProductId);
                }
            }

            var result = products.Select(p => new ProductVM()
            {
                ProductId = p.ProductId,
                Price = p.Price,
                ProductName = p.ProductName,
                Image = p.Image,
                Description = p.Description,
            }).ToList();

            var pagedResult = result.ToPagedList(pageNumber, pageSize);

            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("ListProducts", pagedResult);
            }

            return View(pagedResult);
        }


        public IActionResult QuickView(int ProductId)
        {
            var result = db.Products.Where(p => p.ProductId == ProductId).Select(p => new ProductVM()
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Image = p.Image,
                Description = p.Description,
                Price = p.Price,
            }).FirstOrDefault();
            return Json(result);
        }

        [HttpGet("/viewdetail")]
        public IActionResult ViewDetail(int productId)
        {
            var result = db.Products.FirstOrDefault(p => p.ProductId == productId);
            return View(result);
        }
    }
}
