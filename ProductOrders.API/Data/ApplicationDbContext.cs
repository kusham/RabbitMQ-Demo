using Microsoft.EntityFrameworkCore;
using ProductOrders.API.Models;

namespace ProductOrders.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().HasData(new List<Order>
            {
                new Order
                {
                    Id = Guid.NewGuid(),
                    Amount = 100,
                    CreatedAt = DateTime.UtcNow,
                    CustomerName = "John Doe",
                    CustomerEmail = "",
                    ShippingAddress = "123 Main St, New York, NY 10030",
                    Status = OrderStatus.Pending
                },
                new Order
                {
                    Id = Guid.NewGuid(),
                    Amount = 200,
                    CreatedAt = DateTime.UtcNow,
                    CustomerName = "Jane Doe",
                    CustomerEmail = "",
                    ShippingAddress = "456 Main St, New York, NY 10030",
                    Status = OrderStatus.Pending
                }
            });
        }
    }
}
