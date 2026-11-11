using KASHOP.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplictionUser>
    {
         public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryTranslation> CategoriesTranslation { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductTranslation> ProductTranslation { get; set; }

        public DbSet<Brand> Brands { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { 
        
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.Price).HasPrecision(9, 2);
                entity.HasOne(p => p.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            });
            modelBuilder.Entity<ProductTranslation>(entity =>
            {
                entity.Property(t => t.Name).HasMaxLength(200);
                entity.Property(t => t.Language).HasMaxLength(5);

            });
        }

    }
}
