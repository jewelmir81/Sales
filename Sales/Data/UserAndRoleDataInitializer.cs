using Microsoft.AspNetCore.Identity;
using Sales.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Data
{
    public static class UserAndRoleDataInitializer
    {
        public static void SeedData(UserManager<IdentityUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        private static void SeedUsers(UserManager<IdentityUser> userManager)
        {
            if (userManager.FindByNameAsync("admin").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@sales.com",
                    NormalizedEmail = "ADMIN@SALES.COM",
                    PhoneNumber = "+8801817015015",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                };

                IdentityResult result = userManager.CreateAsync(user, "123456").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }


        }

        private static void SeedRoles(RoleManager<ApplicationRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                ApplicationRole role = new ApplicationRole("Admin", "All", "All");
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }
    }
}
