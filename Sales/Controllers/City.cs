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
    public class City : Controller
    {
        private readonly ApplicationDbContext _context;

        public City(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            ViewData["Create"] = RolesForMenu.GetMenu(User.Identity.Name, "City", "Create");
            ViewData["Edit"] = RolesForMenu.GetMenu(User.Identity.Name, "City", "Edit");
            ViewData["Delete"] = RolesForMenu.GetMenu(User.Identity.Name, "City", "Delete");

            var applicationDbContext = _context.Cities.Include(c => c.Country);
            return View(await applicationDbContext.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities
                .Include(c => c.Country)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        [HttpGet]
        [CustomAuthorize("City", "Create")]
        public IActionResult Create()
        {
            ViewData["CountryId"] = new SelectList(_context.countries, "ID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("City", "Create")]
        public async Task<IActionResult> Create([Bind("ID,Name,CountryId")] Models.City city)
        {
            if (ModelState.IsValid)
            {
                _context.Add(city);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryId"] = new SelectList(_context.countries, "ID", "Name", city.CountryId);
            return View(city);
        }

        [HttpGet]
        [CustomAuthorize("City", "Edit")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_context.countries, "ID", "Name", city.CountryId);
            return View(city);
        }

        [HttpPost]
        [CustomAuthorize("City", "Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,Name,CountryId")] Models.City city)
        {
            if (id != city.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(city);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CityExists(city.ID))
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
            ViewData["CountryId"] = new SelectList(_context.countries, "ID", "Name", city.CountryId);
            return View(city);
        }

        [HttpGet]
        [CustomAuthorize("City", "Delete")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities
                .Include(c => c.Country)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        [HttpPost, ActionName("Delete")]
        [CustomAuthorize("City", "Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var city = await _context.Cities.FindAsync(id);
            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CityExists(long id)
        {
            return _context.Cities.Any(e => e.ID == id);
        }
    }
}
