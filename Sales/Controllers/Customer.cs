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
    public class Customer : Controller
    {
        private readonly ApplicationDbContext _context;

        public Customer(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            ViewData["Create"] = RolesForMenu.GetMenu(User.Identity.Name, "Customer", "Create");
            ViewData["Edit"] = RolesForMenu.GetMenu(User.Identity.Name, "Customer", "Edit");
            ViewData["Delete"] = RolesForMenu.GetMenu(User.Identity.Name, "Customer", "Delete");

            var customers = _context.Customers.Include(c => c.City).ThenInclude(e => e.Country);
            return View(await customers.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.City).ThenInclude(c => c.Country)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpGet]
        [CustomAuthorize("Customer", "Create")]
        public IActionResult Create()
        {
            ViewData["Country"] = new SelectList(_context.countries, "ID", "Name");
            ViewData["CityId"] = new SelectList(_context.Cities.Include(e => e.Country).ToList(), "ID", "Name", null, "Country.Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Customer", "Create")]
        public async Task<IActionResult> Create([Bind("ID,FirstName,LastName,CityId,Phone")] Models.Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Country"] = new SelectList(_context.countries, "ID", "Name");
            ViewData["CityId"] = new SelectList(_context.Cities, "ID", "Name", customer.CityId);
            return View(customer);
        }

        [HttpGet]
        [CustomAuthorize("Customer", "Edit")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["Country"] = new SelectList(_context.countries, "ID", "Name");
            ViewData["CityId"] = new SelectList(_context.Cities.Include(e => e.Country).ToList(), "ID", "Name", customer.CityId, "Country.Name");
            return View(customer);
        }

        [HttpPost]
        [CustomAuthorize("Customer", "Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,FirstName,LastName,CityId,Phone")] Models.Customer customer)
        {
            if (id != customer.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.ID))
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
            ViewData["Country"] = new SelectList(_context.countries, "ID", "Name", customer.City.CountryId);
            ViewData["CityId"] = new SelectList(_context.Cities.Include(e => e.Country).ToList(), "ID", "Name", customer.CityId, "Country.Name");
            return View(customer);
        }

        [HttpGet]
        [CustomAuthorize("Customer", "Delete")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.City).ThenInclude(c => c.Country)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [CustomAuthorize("Customer", "Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public JsonResult GetCities(long countryId)
        {
            List<Models.City> cities = _context.Cities.Include(e => e.Country).Where(e => e.CountryId == countryId).ToList();
            return Json(new SelectList(cities, "ID", "Name", null, "Country.Name"));
        }

        private bool CustomerExists(long id)
        {
            return _context.Customers.Any(e => e.ID == id);
        }
    }
}
