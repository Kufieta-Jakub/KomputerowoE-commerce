using Microsoft.EntityFrameworkCore;
using KomputerowoE_commerce.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KomputerowoE_commerce.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> product { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderProduct> orderProduct { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("product");
            modelBuilder.Entity<Order>().ToTable("order");
            modelBuilder.Entity<OrderProduct>().ToTable("orderproduct");

            modelBuilder.Entity<OrderProduct>()
                .HasKey(op => new { op.OrderId, op.ProductId });

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Order)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.OrderId);

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Product)
                .WithMany(p => p.OrderProducts)
                .HasForeignKey(op => op.ProductId);
        }
    }
}