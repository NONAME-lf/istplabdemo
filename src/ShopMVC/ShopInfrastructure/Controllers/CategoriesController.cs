using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopDomain.Model;
using ShopInfrastructure;

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
            var shopDbContext = _context.ProductCatergories.Include(p => p.PctCategory).Include(p => p.PctProduct);
            return View(await shopDbContext.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCatergory = await _context.ProductCatergories
                .Include(p => p.PctCategory)
                .Include(p => p.PctProduct)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productCatergory == null)
            {
                return NotFound();
            }

            return View(productCatergory);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            ViewData["PctCategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            ViewData["PctProductId"] = new SelectList(_context.Products, "Id", "Id");
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PctProductId,PctCategoryId,Id")] ProductCatergory productCatergory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productCatergory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PctCategoryId"] = new SelectList(_context.Categories, "Id", "Id", productCatergory.PctCategoryId);
            ViewData["PctProductId"] = new SelectList(_context.Products, "Id", "Id", productCatergory.PctProductId);
            return View(productCatergory);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCatergory = await _context.ProductCatergories.FindAsync(id);
            if (productCatergory == null)
            {
                return NotFound();
            }
            ViewData["PctCategoryId"] = new SelectList(_context.Categories, "Id", "Id", productCatergory.PctCategoryId);
            ViewData["PctProductId"] = new SelectList(_context.Products, "Id", "Id", productCatergory.PctProductId);
            return View(productCatergory);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PctProductId,PctCategoryId,Id")] ProductCatergory productCatergory)
        {
            if (id != productCatergory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productCatergory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductCatergoryExists(productCatergory.Id))
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
            ViewData["PctCategoryId"] = new SelectList(_context.Categories, "Id", "Id", productCatergory.PctCategoryId);
            ViewData["PctProductId"] = new SelectList(_context.Products, "Id", "Id", productCatergory.PctProductId);
            return View(productCatergory);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCatergory = await _context.ProductCatergories
                .Include(p => p.PctCategory)
                .Include(p => p.PctProduct)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productCatergory == null)
            {
                return NotFound();
            }

            return View(productCatergory);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productCatergory = await _context.ProductCatergories.FindAsync(id);
            if (productCatergory != null)
            {
                _context.ProductCatergories.Remove(productCatergory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductCatergoryExists(int id)
        {
            return _context.ProductCatergories.Any(e => e.Id == id);
        }
    }
}
