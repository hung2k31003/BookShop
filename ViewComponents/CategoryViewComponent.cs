using BookShopV2.Models;
using BookShopV2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShopV2.ViewComponents
{
    public class CategoryViewComponent:ViewComponent
    {
        private readonly CategorySevice _categorySevice;
        public CategoryViewComponent( CategorySevice categorySevice) 
        {
            _categorySevice = categorySevice;
        }

        public IViewComponentResult Invoke()
        {
            return View("Category",_categorySevice.GetCategories());
        }
    }
}
