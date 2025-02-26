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
    public class OrdersController : Controller
    {
        private readonly ShopDbContext _context;

        public OrdersController(ShopDbContext context)
        {
            _context = context;
        }

        public Order CreateOrder(int userId)
        {
            Console.WriteLine("Оберіть спосіб оплати (1 - Credit Card, 2 - PayPal, 3 - Cash on Delivery):");
            string? paymentMethod = Console.ReadLine();
    
            Console.WriteLine("Додайте нотатки (якщо потрібно):");
            string? notes = Console.ReadLine();
    
            Console.WriteLine("Оберіть спосіб доставки (1 - DHL, 2 - FedEx, 3 - UPS):");
            int shippingOption = int.Parse(Console.ReadLine());

            var shipping = new Shiping
            {
                ShippingCompanyId = shippingOption
            };

            var order = new Order
            {
                //UserId = userId,
                OdPayment = paymentMethod,
                OdNotes = notes,
                Shipping = shipping
            };

            // Додати замовлення в базу
            _context.Orders.Add(order);
            _context.SaveChanges();

            return order;
        }
        
        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var shopDbContext = _context.Orders.Include(o => o.OdUserNavigation).Include(o => o.Protuct).Include(o => o.Receipt).Include(o => o.Shipping);
            return View(await shopDbContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.OdUserNavigation)
                .Include(o => o.Protuct)
                .Include(o => o.Receipt)
                .Include(o => o.Shipping)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["OdUser"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["ProtuctId"] = new SelectList(_context.Products, "Id", "PdName");
            ViewData["ReceiptId"] = new SelectList(_context.Receipts, "Id", "Id");
            ViewData["ShippingId"] = new SelectList(_context.Shipings, "Id", "Id");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OdUser,OdTotal,OdDiscount,OdPayment,OdNotes,ReceiptId,ProtuctId,ShippingId,Id")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OdUser"] = new SelectList(_context.Users, "Id", "Id", order.OdUser);
            ViewData["ProtuctId"] = new SelectList(_context.Products, "Id", "PdName", order.ProtuctId);
            ViewData["ReceiptId"] = new SelectList(_context.Receipts, "Id", "Id", order.ReceiptId);
            ViewData["ShippingId"] = new SelectList(_context.Shipings, "Id", "Id", order.ShippingId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["OdUser"] = new SelectList(_context.Users, "Id", "Id", order.OdUser);
            ViewData["ProtuctId"] = new SelectList(_context.Products, "Id", "PdName", order.ProtuctId);
            ViewData["ReceiptId"] = new SelectList(_context.Receipts, "Id", "Id", order.ReceiptId);
            ViewData["ShippingId"] = new SelectList(_context.Shipings, "Id", "Id", order.ShippingId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OdUser,OdTotal,OdDiscount,OdPayment,OdNotes,ReceiptId,ProtuctId,ShippingId,Id")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            ViewData["OdUser"] = new SelectList(_context.Users, "Id", "Id", order.OdUser);
            ViewData["ProtuctId"] = new SelectList(_context.Products, "Id", "PdName", order.ProtuctId);
            ViewData["ReceiptId"] = new SelectList(_context.Receipts, "Id", "Id", order.ReceiptId);
            ViewData["ShippingId"] = new SelectList(_context.Shipings, "Id", "Id", order.ShippingId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.OdUserNavigation)
                .Include(o => o.Protuct)
                .Include(o => o.Receipt)
                .Include(o => o.Shipping)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
