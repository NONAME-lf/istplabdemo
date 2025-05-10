using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopDomain.Model;
using ShopInfrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace ShopInfrastructure.Controllers
{
    [Route("[controller]")]
    public class QueriesController : Controller
    {
        private readonly ShopDbContext _context;

        public QueriesController(ShopDbContext context)
        {
            _context = context;
        }

        // GET: Queries
        [HttpGet]
        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }

        // Query 1: Find products that haven't been ordered yet and have quantity less than specified
        [HttpGet]
        [Route("UnorderedProductsByQuantity")]
        public async Task<IActionResult> UnorderedProductsByQuantity(int maxQuantity)
        {
            var products = await _context.Products
                .Where(p => p.PdQuantity < maxQuantity &&
                           !_context.ProductOrders.Any(po => po.ProductId == p.Id))
                .Select(p => new
                {
                    p.PdName,
                    p.PdQuantity,
                    p.PdPrice,
                    Manufacturer = p.Manufacturer.MnName
                })
                .ToListAsync();

            return View(products);
        }

        // Query 2: Find manufacturers that supply products ordered by a specific user
        [HttpGet]
        [Route("ManufacturersByUserOrders")]
        public async Task<IActionResult> ManufacturersByUserOrders(string userId)
        {
            var manufacturers = await _context.Manufacturers
                .Where(m => m.Products
                    .Any(p => p.ProductOrders
                        .Any(po => po.Order.OdUser == userId)))
                .Select(m => new
                {
                    m.MnName,
                    Products = m.Products
                        .Where(p => p.ProductOrders
                            .Any(po => po.Order.OdUser == userId))
                        .Select(p => p.PdName)
                })
                .ToListAsync();

            return View(manufacturers);
        }

        // Query 3: Find products in the same categories as a specified product
        [HttpGet]
        [Route("ProductsInSameCategories")]
        public async Task<IActionResult> ProductsInSameCategories(int productId)
        {
            var products = await _context.Products
                .Where(p => p.Id != productId &&
                           p.ProductCategories
                               .Select(pc => pc.CategoryId)
                               .All(catId => _context.ProductCategories
                                   .Where(pc => pc.ProductId == productId)
                                   .Select(pc => pc.CategoryId)
                                   .Contains(catId)))
                .Select(p => new
                {
                    p.PdName,
                    Categories = p.ProductCategories
                        .Select(pc => pc.Category.CgName)
                })
                .ToListAsync();

            return View(products);
        }

        // Query 4: Find users who haven't ordered any products that a specific user has ordered
        [HttpGet]
        [Route("UsersWithDifferentOrders")]
        public async Task<IActionResult> UsersWithDifferentOrders(string userId)
        {
            var users = await _context.Users
                .Where(u => u.Id != userId &&
                           !u.Orders
                               .SelectMany(o => o.ProductOrders)
                               .Select(po => po.ProductId)
                               .Any(pid => _context.Orders
                                   .Where(o => o.OdUser == userId)
                                   .SelectMany(o => o.ProductOrders)
                                   .Select(po => po.ProductId)
                                   .Contains(pid)))
                .Select(u => new
                {
                    u.UserName,
                    OrderCount = u.Orders.Count
                })
                .ToListAsync();

            return View(users);
        }

        // Query 5: Find products with price higher than average in their category
        [HttpGet]
        [Route("ProductsAboveCategoryAverage")]
        public async Task<IActionResult> ProductsAboveCategoryAverage()
        {
            var products = await _context.Products
                .Select(p => new
                {
                    p.PdName,
                    p.PdPrice,
                    Category = p.ProductCategories
                        .Select(pc => pc.Category.CgName)
                        .FirstOrDefault(),
                    AveragePrice = _context.Products
                        .Where(p2 => p2.ProductCategories
                            .Any(pc => pc.CategoryId == p.ProductCategories
                                .Select(pc2 => pc2.CategoryId)
                                .FirstOrDefault()))
                        .Average(p2 => p2.PdPrice)
                })
                .Where(p => p.PdPrice > p.AveragePrice)
                .ToListAsync();

            return View(products);
        }

        // Query 6: Find manufacturers that supply all products in a specific category
        [HttpGet]
        [Route("ManufacturersWithAllCategoryProducts")]
        public async Task<IActionResult> ManufacturersWithAllCategoryProducts(int categoryId)
        {
            var manufacturers = await _context.Manufacturers
                .Where(m => m.Products
                    .SelectMany(p => p.ProductCategories)
                    .Where(pc => pc.CategoryId == categoryId)
                    .Select(pc => pc.ProductId)
                    .Distinct()
                    .Count() == _context.Products
                        .Where(p => p.ProductCategories
                            .Any(pc => pc.CategoryId == categoryId))
                        .Count())
                .Select(m => new
                {
                    m.MnName,
                    Products = m.Products
                        .Where(p => p.ProductCategories
                            .Any(pc => pc.CategoryId == categoryId))
                        .Select(p => p.PdName)
                })
                .ToListAsync();

            return View(manufacturers);
        }

        // Query 7: Find pairs of users who ordered exactly the same products
        [HttpGet]
        [Route("UsersWithSameOrders")]
        public async Task<IActionResult> UsersWithSameOrders()
        {
            var userPairs = await _context.Users
                .SelectMany(u1 => _context.Users
                    .Where(u2 => u1.Id.CompareTo(u2.Id) < 0)
                    .Select(u2 => new { User1 = u1, User2 = u2 }))
                .Where(pair =>
                    pair.User1.Orders
                        .SelectMany(o => o.ProductOrders)
                        .Select(po => po.ProductId)
                        .OrderBy(id => id)
                        .SequenceEqual(
                            pair.User2.Orders
                                .SelectMany(o => o.ProductOrders)
                                .Select(po => po.ProductId)
                                .OrderBy(id => id)))
                .Select(pair => new
                {
                    User1Name = pair.User1.UserName,
                    User2Name = pair.User2.UserName,
                    CommonProducts = pair.User1.Orders
                        .SelectMany(o => o.ProductOrders)
                        .Select(po => po.Product.PdName)
                        .Distinct()
                })
                .ToListAsync();

            return View(userPairs);
        }
    }
}