using System.ComponentModel.DataAnnotations;

namespace ProductOrders.API.Models
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public required string CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public required string ShippingAddress { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
    }

    public enum OrderStatus
    {
        Pending,
        Shipped,
        Delivered,
        Cancelled
    }
}
