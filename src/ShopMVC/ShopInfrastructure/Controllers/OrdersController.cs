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

        [HttpPost]
        public async Task<IActionResult> Checkout(string paymentMethod, string? cardNumber, string? orderNotes,
            string shippingMethod, string? address, int? shippingCompanyId)
        {
            var cartId = HttpContext.Session.GetInt32("CartId");
            if (cartId == null) return RedirectToAction("Index", "Carts");

            var cart = await _context.Carts
                .Include(c => c.ProductCarts)
                .ThenInclude(pc => pc.Product)
                .FirstOrDefaultAsync(c => c.Id == cartId);

            if (cart == null || !cart.ProductCarts.Any())
            {
                return RedirectToAction("Index", "Carts");
            }

            // **Перевірка наявності всіх товарів перед оформленням**
            foreach (var productCart in cart.ProductCarts)
            {
                if (productCart.Product.PdQuantity < productCart.PcQuantity)
                {
                    TempData["Error"] = $"Товар {productCart.Product.PdName} закінчився!";
                    return RedirectToAction("Index", "Carts");
                }
            }

            // Якщо самовивіз — встановити адресу "Самовивіз"
            address = shippingMethod == "pickup" ? "Самовивіз" : address;

            var shipping = new Shiping
            {
                ShAdress = address,
                ShippingCompanyId = shippingMethod == "delivery" ? shippingCompanyId : null
            };
            _context.Shipings.Add(shipping);
            await _context.SaveChangesAsync();

            var order = new Order
            {
                OdUser = null,
                OdTotal = cart.ProductCarts.Sum(pc => pc.PcPrice),
                OdPayment = paymentMethod == "card" ? cardNumber : "cash",
                OdNotes = orderNotes,
                ShippingId = shipping.Id,
                ProductOrders = cart.ProductCarts.Select(pc => new ProductOrder
                {
                    ProductId = pc.ProductId,
                    PoPrice = pc.PcPrice,
                    PoQuantity = pc.PcQuantity
                }).ToList()
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var receipt = new Receipt
            {
                RpDateCreated = DateTime.Now,
                RpQuantity = cart.ProductCarts.Sum(pc => pc.PcQuantity),
                RpTotal = order.OdTotal,
                RpPayment = order.OdPayment,
                RpAbout = order.OdNotes,
                ShippingId = shipping.Id,
                Orders = new List<Order> { order }
            };
            _context.Receipts.Add(receipt);
            await _context.SaveChangesAsync();

            // **Віднімаємо товари з наявної кількості**
            foreach (var productCart in cart.ProductCarts)
            {
                var product = await _context.Products.FindAsync(productCart.ProductId);
                if (product != null)
                {
                    product.PdQuantity -= productCart.PcQuantity; // **Зменшуємо кількість**
                }
            }

            await _context.SaveChangesAsync();

            // Очищення кошика
            _context.ProductCarts.RemoveRange(cart.ProductCarts);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public class OrderViewModel
        {
            public int Id { get; set; }
            public decimal? OdTotal { get; set; }
            public string? OdPayment { get; set; }
            public string? OdNotes { get; set; }
            public List<ProductViewModel> Products { get; set; } = new List<ProductViewModel>();
            public ShippingViewModel? Shipping { get; set; }
            public ReceiptViewModel? Receipt { get; set; }
        }

        public class ProductViewModel
        {
            public string Name { get; set; }
            public decimal? Price { get; set; }
            public string Description { get; set; }
        }

        public class ShippingViewModel
        {
            public string Address { get; set; }
            public string ShippingCompany { get; set; }
        }

        public class ReceiptViewModel
        {
            public DateTime? DateCreated { get; set; }
            public int? Quantity { get; set; }
            public decimal? Total { get; set; }
            public string? Payment { get; set; }
            public string? About { get; set; }
        }

        
        
        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var shopDbContext = _context.Orders.Include(o => o.OdUserNavigation).Include(o => o.Product).Include(o => o.Receipt).Include(o => o.Shipping);
            return View(await shopDbContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var orderWithDetails = await _context.Orders
                .Where(o => o.Id == id)
                .Include(o => o.ProductOrders)
                .ThenInclude(po => po.Product)
                .Include(o => o.Shipping)
                .ThenInclude(s => s.ShippingCompany)
                .Include(o => o.Receipt)
                .Select(o => new OrderViewModel
                {
                    Id = o.Id,
                    OdTotal = o.OdTotal,
                    OdPayment = o.OdPayment,
                    OdNotes = o.OdNotes,
                    Products = o.ProductOrders.Select(po => new ProductViewModel
                    {
                        Name = po.Product.PdName,
                        Price = po.Product.PdPrice,
                        Description = po.Product.PdAbout
                    }).ToList(),
                    Shipping = o.Shipping != null ? new ShippingViewModel
                    {
                        Address = o.Shipping.ShAdress,
                        ShippingCompany = o.Shipping.ShippingCompany != null ? o.Shipping.ShippingCompany.ScName : "Немає"
                    } : null,
                    Receipt = o.Receipt != null ? new ReceiptViewModel
                    {
                        DateCreated = o.Receipt.RpDateCreated,
                        Quantity = o.Receipt.RpQuantity,
                        Total = o.Receipt.RpTotal,
                        Payment = o.Receipt.RpPayment,
                        About = o.Receipt.RpAbout
                    } : null
                })
                .FirstOrDefaultAsync();

            if (orderWithDetails == null)
            {
                return NotFound();
            }

            return View(orderWithDetails);
        }


        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["OdUser"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "PdName");
            ViewData["ReceiptId"] = new SelectList(_context.Receipts, "Id", "Id");
            ViewData["ShippingId"] = new SelectList(_context.Shipings, "Id", "Id");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OdUser,OdTotal,OdDiscount,OdPayment,OdNotes,ReceiptId,ProductId,ShippingId,Id")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OdUser"] = new SelectList(_context.Users, "Id", "Id", order.OdUser);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "PdName", order.ProductId);
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
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "PdName", order.ProductId);
            ViewData["ReceiptId"] = new SelectList(_context.Receipts, "Id", "Id", order.ReceiptId);
            ViewData["ShippingId"] = new SelectList(_context.Shipings, "Id", "Id", order.ShippingId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OdUser,OdTotal,OdDiscount,OdPayment,OdNotes,ReceiptId,ProductId,ShippingId,Id")] Order order)
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
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "PdName", order.ProductId);
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
                .Include(o => o.Product)
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
                // Видаляємо всі записи з ProductOrders, які пов'язані з цим Order
                var productOrders = _context.ProductOrders.Where(po => po.OrderId == id);
                _context.ProductOrders.RemoveRange(productOrders);

                // Видаляємо саме замовлення
                _context.Orders.Remove(order);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
