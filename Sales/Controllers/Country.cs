using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sales.Attributes.Authorize;
using Sales.Data;
using Sales.Models;

namespace Sales.Controllers
{
    [Authorize]
    public class Country : Controller
    {
        private readonly ApplicationDbContext _context;

        public Country(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            ViewData["Create"] = RolesForMenu.GetMenu(User.Identity.Name, "Country", "Create");
            ViewData["Edit"] = RolesForMenu.GetMenu(User.Identity.Name, "Country", "Edit");
            ViewData["Delete"] = RolesForMenu.GetMenu(User.Identity.Name, "Country", "Delete");

            return View(await _context.countries.Include(e => e.Cities).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.countries
                .FirstOrDefaultAsync(m => m.ID == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        [HttpGet]
        [CustomAuthorize("Country", "Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [CustomAuthorize("Country", "Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Cities")] Models.Country country)
        {
            if (ModelState.IsValid)
            {
                _context.Add(country);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(country);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.countries.Include(c => c.Cities).FirstAsync(c => c.ID == id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,Name,Cities")] Models.Country country)
        {
            if (id != country.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountryExists(country.ID))
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
            return View(country);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.countries
                .FirstOrDefaultAsync(m => m.ID == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var country = await _context.countries.FindAsync(id);
            _context.countries.Remove(country);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CountryExists(long id)
        {
            return _context.countries.Any(e => e.ID == id);
        }
    }
}
