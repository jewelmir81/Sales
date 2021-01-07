using Microsoft.EntityFrameworkCore;
using Sales.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Data
{
    public static class RolesForMenu
    {
        public static List<Menu> GetMenus(string userName)
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=SalesDB").Options;
            ApplicationDbContext _context = new ApplicationDbContext(contextOptions);
            var userId = _context.Users.Single(e => e.UserName == userName).Id;

            if (_context.Roles.Any(r => _context.UserRoles.Any(ur => ur.UserId == userId && ur.RoleId == r.Id) && r.Name == "Admin"))
            {
                return _context.Roles.Where(e => e.ActionName == "Index").Select(e => new Menu(e.Name, e.ControllerName, e.ActionName)).ToList();
            }
            else
            {
                return _context.Roles.Where(r => _context.UserRoles.Any(ur => ur.UserId == userId && ur.RoleId == r.Id) && r.ActionName == "Index").Select(e => new Menu(e.Name, e.ControllerName, e.ActionName)).ToList();
            }
        }

        public static Menu GetMenu(string userName, string controller, string action)
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=SalesDB").Options;
            ApplicationDbContext _context = new ApplicationDbContext(contextOptions);
            var userId = _context.Users.Single(e => e.UserName == userName).Id;

            if (_context.Roles.Any(r => _context.UserRoles.Any(ur => ur.UserId == userId && ur.RoleId == r.Id && r.ControllerName == controller && r.ActionName == action)))
            {
                return _context.Roles.Where(e => e.ControllerName == controller && e.ActionName == action).Select(e => new Menu(e.Name, e.ControllerName, e.ActionName)).First();
            }
            else
            {
                return null;
            }
        }
    }
}
