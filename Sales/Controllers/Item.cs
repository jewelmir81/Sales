using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sales.Attributes.Authorize;
using Sales.Data;
using Sales.Models;

namespace Sales.Controllers
{
    [Authorize]
    public class Item : Controller
    {
        private readonly ApplicationDbContext _context;

        public Item(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            ViewData["Create"] = RolesForMenu.GetMenu(User.Identity.Name, "Item", "Create");
            ViewData["Edit"] = RolesForMenu.GetMenu(User.Identity.Name, "Item", "Edit");
            ViewData["Delete"] = RolesForMenu.GetMenu(User.Identity.Name, "Item", "Delete");

            var applicationDbContext = _context.OrderItems.Include(o => o.Order).Include(o => o.Product);
            return View(await applicationDbContext.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItems
                .Include(o => o.Order)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        [HttpGet]
        [CustomAuthorize("Item", "Create")]
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Orders, "ID", "OrderNumber");
            ViewData["ProductId"] = new SelectList(_context.Products, "ID", "ProductName");
            return View();
        }

        [HttpPost]
        [CustomAuthorize("Item", "Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,OrderId,ProductId,UnitPrice,Quantity")] OrderItem orderItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "ID", "OrderNumber", orderItem.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ID", "ProductName", orderItem.ProductId);
            return View(orderItem);
        }

        [HttpGet]
        [CustomAuthorize("Item", "Edit")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "ID", "OrderNumber", orderItem.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ID", "ProductName", orderItem.ProductId);
            return View(orderItem);
        }

        [HttpPost]
        [CustomAuthorize("Item", "Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,OrderId,ProductId,UnitPrice,Quantity")] OrderItem orderItem)
        {
            if (id != orderItem.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderItemExists(orderItem.ID))
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
            ViewData["OrderId"] = new SelectList(_context.Orders, "ID", "OrderNumber", orderItem.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ID", "ProductName", orderItem.ProductId);
            return View(orderItem);
        }

        [HttpGet]
        [CustomAuthorize("Item", "Delete")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItems
                .Include(o => o.Order)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        [HttpPost, ActionName("Delete")]
        [CustomAuthorize("Item", "Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);
            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderItemExists(long id)
        {
            return _context.OrderItems.Any(e => e.ID == id);
        }
    }
}
