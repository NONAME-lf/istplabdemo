using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopDomain;
using ShopDomain.Model;

namespace ShopInfrastructure.Controllers
{
    public class CartsController : Controller
    {
        private readonly ShopDbContext _context;

        public CartsController(ShopDbContext context)
        {
            _context = context;
        }

        
        public async Task<Cart> GetOrCreateCart()
        {
            var cartId = HttpContext.Session.GetInt32("CartId");

            if (cartId == null)
            {
                var newCart = new Cart();
                _context.Carts.Add(newCart);
                await _context.SaveChangesAsync();

                HttpContext.Session.SetInt32("CartId", newCart.Id);
                return newCart;
            }

            var cart = await _context.Carts
                .Include(c => c.ProductCarts)
                .ThenInclude(pc => pc.Product)
                .FirstOrDefaultAsync(c => c.Id == cartId);

            return cart ?? throw new Exception("Кошик не знайдено.");
        }
        
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return NotFound();

            var cart = await _context.Carts
                .Include(c => c.ProductCarts)
                .FirstOrDefaultAsync();

            if (cart == null)
            {
                cart = new Cart();
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var productCart = cart.ProductCarts.FirstOrDefault(pc => pc.ProductId == productId);
            if (productCart == null)
            {
                productCart = new ProductCart { Cart = cart, Product = product, PcQuantity = 1, PcPrice = product.PdPrice };
                cart.ProductCarts.Add(productCart);
            }
            else
            {
                productCart.PcQuantity++;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Carts");
        }


        [HttpGet]
        // GET: Carts
        public async Task<IActionResult> Index(/*int? id, string? name*/)
        {
            var cart = await _context.Carts
                .Include(c => c.ProductCarts)
                .ThenInclude(pc => pc.Product)
                .FirstOrDefaultAsync(); // Поки без користувачів, просто беремо перший кошик
    
            return View(cart);
            
            // if (id == null) return RedirectToAction("Index", "Categories");
            // ViewBag.ProductId = id;
            // ViewBag.ProductName = name;
            // var productInCart = _context.Carts
            //     .Include(p => p.ProductCarts)
            //     .ThenInclude(pc => pc.Product)
            //     //.Include(pc => pc.Manufacturer)
            //     .Where(b => b.ProductCarts.Any(pc => pc.ProductId == id));
            //
            // return View(await productInCart.ToListAsync());
            
            // var cartItems = await _context.ProductCarts
            //     .Include(pc => pc.Product)
            //     .ToListAsync();
            // return View(cartItems);
            
            //var shopDbContext = _context.Carts.Include(c => c.User);
            //return View(await shopDbContext.ToListAsync());
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UrNickname");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CtQuantity,CtPrice,UserId,Id")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UrNickname", cart.UserId);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UrNickname", cart.UserId);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CtQuantity,CtPrice,UserId,Id")] Cart cart)
        {
            if (id != cart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UrNickname", cart.UserId);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }
    }
}
