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
    public class Product : Controller
    {
        private readonly ApplicationDbContext _context;

        public Product(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            ViewData["Create"] = RolesForMenu.GetMenu(User.Identity.Name, "Product", "Create");
            ViewData["Edit"] = RolesForMenu.GetMenu(User.Identity.Name, "Product", "Edit");
            ViewData["Delete"] = RolesForMenu.GetMenu(User.Identity.Name, "Product", "Delete");

            var applicationDbContext = _context.Products.Include(p => p.Supplier);
            return View(await applicationDbContext.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpGet]
        [CustomAuthorize("Product", "Create")]
        public IActionResult Create()
        {
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "ID", "CompanyName");
            return View();
        }

        [HttpPost]
        [CustomAuthorize("Product", "Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ProductName,SupplierId,UnitPrice,Package,IsDiscontinued")] Models.Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "ID", "CompanyName", product.SupplierId);
            return View(product);
        }

        [HttpGet]
        [CustomAuthorize("Product", "Edit")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "ID", "CompanyName", product.SupplierId);
            return View(product);
        }

        [HttpPost]
        [CustomAuthorize("Product", "Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,ProductName,SupplierId,UnitPrice,Package,IsDiscontinued")] Models.Product product)
        {
            if (id != product.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ID))
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
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "ID", "CompanyName", product.SupplierId);
            return View(product);
        }

        [HttpGet]
        [CustomAuthorize("Product", "Delete")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [CustomAuthorize("Product", "Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(long id)
        {
            return _context.Products.Any(e => e.ID == id);
        }
    }
}
