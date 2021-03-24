using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace PriceTracker.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<UserProduct> UserProduct { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Products)
                .WithMany(u => u.Users)
                .UsingEntity<UserProduct>(
                    j => j
                        .HasOne(up => up.Product)
                        .WithMany(p => p.UserProducts)
                        .HasForeignKey(up => up.ProductId),
                    j => j
                        .HasOne(up => up.User)
                        .WithMany(p => p.UserProducts)
                        .HasForeignKey(up => up.UserId),
                    j =>
                    {
                        j.HasKey(u => new { u.UserId, u.ProductId });
                    });


        }
    }
}
