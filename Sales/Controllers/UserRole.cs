using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sales.Data;
using Sales.Models;
using Sales.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserRole : Controller
    {
        private RoleManager<ApplicationRole> _roleManager;
        private UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UserRole(RoleManager<ApplicationRole> roleManager, UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            List<ViewModels.UserRole> userRoles = new List<ViewModels.UserRole>();
            List<UserShorthand> users = _userManager.Users.Select(e => new UserShorthand(e.Id, e.UserName)).ToList();
            foreach(UserShorthand user in users)
            {
                List<RoleShorthand> activeRoles = _context.Roles.Where(r => _context.UserRoles.Any(ur => ur.UserId == user.Id && ur.RoleId == r.Id)).Select(e => new RoleShorthand(e.Id, e.Name)).ToList();
                List<RoleShorthand> availableRoles = _context.Roles.Where(r => _context.UserRoles.All(ur => ur.UserId == user.Id && ur.RoleId != r.Id)).Select(e => new RoleShorthand(e.Id, e.Name)).ToList();
                userRoles.Add(new ViewModels.UserRole(user, activeRoles, availableRoles));
            }
            return View(userRoles);
        }

        public IActionResult Create()
        {
            List<UserShorthand> users = _context.Users.Select(e => new UserShorthand(e.Id, e.UserName)).ToList();
            ViewData["Users"] = new SelectList(users, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId", "RoleIds")] ViewModels.UserRole userRole)
        {
            if(ModelState.IsValid)
            {
                IdentityUser user = await _userManager.FindByIdAsync(userRole.UserId);
                List<string> assignedRoleIds = _context.Roles.Where(r => _context.UserRoles.Any(ur => ur.UserId == userRole.UserId && ur.RoleId == r.Id)).Select(e => new string(e.Name)).ToList();
                if (assignedRoleIds != null)
                {
                    await _userManager.RemoveFromRolesAsync(user, assignedRoleIds);
                }
                if (userRole.RoleIds != null)
                {
                    await _userManager.AddToRolesAsync(user, userRole.RoleIds);
                }
                return RedirectToAction(nameof(Index));
            }
            List<UserShorthand> users = _context.Users.Select(e => new UserShorthand(e.Id, e.UserName)).ToList();
            List<string> currentRoleIds = _context.Roles.Where(r => _context.UserRoles.Any(ur => ur.UserId == userRole.UserId && ur.RoleId == r.Id)).Select(e => new string(e.Id)).ToList();
            ViewData["Users"] = new MultiSelectList(users, "Id", "Name", currentRoleIds);
            return View();
        }

        public JsonResult GetRoles(string userId)
        {
            List<RoleShorthand> roles = _context.Roles.Select(e => new RoleShorthand(e.Id, e.Name)).ToList();
            List<string> assignedRoleIdss = _context.Roles.Where(r => _context.UserRoles.Any(ur => ur.UserId == userId && ur.RoleId == r.Id)).Select(e => new string(e.Id)).ToList();
            return Json(new MultiSelectList(roles, "Id", "Name", assignedRoleIdss));
        }
    }
}
