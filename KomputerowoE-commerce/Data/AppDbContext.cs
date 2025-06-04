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
        public DbSet<Orders> orders { get; set; }
        public DbSet<OrderProduct> orderProduct { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("product");
            modelBuilder.Entity<Orders>().ToTable("orders");
            modelBuilder.Entity<OrderProduct>().ToTable("orderproduct");

            modelBuilder.Entity<OrderProduct>()
                .HasKey(op => new { op.orderid, op.productid });

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Orders)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.orderid);

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Product)
                .WithMany(p => p.OrderProducts)
                .HasForeignKey(op => op.productid);
        }
    }
}