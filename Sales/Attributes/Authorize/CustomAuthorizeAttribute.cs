using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Sales.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Sales.Attributes.Authorize
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly ApplicationDbContext _context;
        private readonly string _controller;
        private readonly string _action;

        public CustomAuthorizeAttribute(ApplicationDbContext context)
        {
            _context = context;
        }

        public CustomAuthorizeAttribute(string controller, string action)
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=SalesDB").Options;
            _context = new ApplicationDbContext(contextOptions);
            _controller = controller;
            _action = action;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            var userId = _context.Users.Single(e => e.UserName == user.Identity.Name).Id;

            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            else
            {
                var roles = _context.Roles.Where(r => _context.UserRoles.Any(ur => ur.UserId == userId && ur.RoleId == r.Id)).ToList();

                if(roles.Any(e => e.Name == "Admin"))
                {
                    return;
                }
                else if (roles.Any(e => e.ControllerName == _controller && e.ActionName == _action))
                {
                    return;
                }
                else
                {
                    context.Result = new RedirectResult("~/");
                    return;
                }
            }
        }
    }
}
