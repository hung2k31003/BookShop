using BookShopV2.Models;

namespace BookShopV2.Services
{
    public class CategorySevice
    {
        private readonly AppDbContext _context;
        public CategorySevice(AppDbContext context)
        {
            _context = context;
        }
        public List<Category> GetCategories() 
        {
            return _context.Categories.ToList();
        }
    }
}
