using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sales.Data;
using Sales.Models;

namespace Sales.Controllers
{
    [Authorize(Roles = "Admin")]
    public class Role : Controller
    {
        RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public Role(ApplicationDbContext context, RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.OrderBy(e => e.ControllerName).ThenBy(e => e.ActionName).ToListAsync();
            return View(roles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            List<Type> controllers = Assembly.GetExecutingAssembly().GetTypes().Where(type => typeof(Controller).IsAssignableFrom(type)).ToList();
            ViewData["controllers"] = new SelectList(controllers, "Name", "Name");
            return View(new ApplicationRole());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationRole applicationRole)
        {
            if (ModelState.IsValid)
            {
                await _roleManager.CreateAsync(applicationRole);
                return RedirectToAction("Index");
            }
            List<Type> controllers = Assembly.GetExecutingAssembly().GetTypes().Where(type => typeof(Controller).IsAssignableFrom(type)).ToList();
            ViewData["controllers"] = new SelectList(controllers, "Name", "Name");
            return View(applicationRole);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationRole = await _roleManager.Roles.FirstAsync(e => e.Id == id);
            if (applicationRole == null)
            {
                return NotFound();
            }
            return View(applicationRole);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationRole applicationRole)
        {
            if (id != applicationRole.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ApplicationRole role = await _roleManager.FindByIdAsync(applicationRole.Id);
                    role.Name = applicationRole.Name;
                    IdentityResult result = await _roleManager.UpdateAsync(role);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is Models.ApplicationRole)
                        {
                            if (!ApplicationRoleExists(applicationRole.Id))
                            {
                                return NotFound();
                            }
                            else
                            {
                                var proposedValues = entry.CurrentValues;
                                var databaseValues = entry.GetDatabaseValues();

                                foreach (var property in proposedValues.Properties)
                                {
                                    var proposedValue = proposedValues[property];
                                    var databaseValue = databaseValues[property];
                                }
                                entry.OriginalValues.SetValues(databaseValues);
                            }
                        }
                        else
                        {
                            throw new NotSupportedException("Don't know how to handle concurrency conflicts for " + entry.Metadata.Name);
                        }
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(applicationRole);
        }

        public JsonResult GetActions(string controllerName)
        {
            List<string> actions = Assembly.GetExecutingAssembly().GetTypes().Where(type => typeof(Controller).IsAssignableFrom(type) && type.Name == controllerName)
                .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Where(m => m.GetCustomAttributes(typeof(Microsoft.AspNetCore.Mvc.HttpGetAttribute), true).Any())
                .GroupBy(e => e.Name)
                .Select(x => x.FirstOrDefault().Name).ToList();
            return Json(actions);
        }

        private bool ApplicationRoleExists(string id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
    }
}
