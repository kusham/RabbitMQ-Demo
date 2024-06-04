using Microsoft.EntityFrameworkCore;
using ProductBilling.API.Models;

namespace ProductBilling.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

        public DbSet<Billing> Billings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Billing>().HasData(new List<Billing>
            {
                new Billing
                {
                    Id = Guid.NewGuid(),
                    Amount = 150.75m,
                    CustomerName = "Alice Johnson",
                    BillingAddress = "456 Elm St, Springfield, USA",
                    CustomerEmail = "alice.johnson@example.com",
                    PaymentMethod = PaymentMethod.PayPal,
                    Currency = Currency.USD,
                    Notes = "First payment"
                },
                new Billing
                {
                    Id = Guid.NewGuid(),
                    Amount = 299.99m,
                    CustomerName = "Bob Smith",
                    BillingAddress = "789 Oak St, Metropolis, USA",
                    CustomerEmail = "bob.smith@example.com",
                    PaymentMethod = PaymentMethod.CreditCard,
                    Currency = Currency.USD,
                    Notes = "Second payment"
                 }
        });
        }
    }
}
