using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopDomain.Model;
using ShopInfrastructure;
using ShopInfrastructure; // Простір імен для DbContext

namespace ShopInfrastructure.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ShopDbContext _context;

        public CategoriesController(ShopDbContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }
    }
}