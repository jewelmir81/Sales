using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sales.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sales.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<Country> countries { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Product>().HasMany(e => e.OrderItems).WithOne(e => e.Product).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Order>().HasMany(e => e.OrderItems).WithOne(e => e.Order).OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(builder);
        }
    }
}
