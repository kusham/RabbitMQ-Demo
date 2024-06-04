

namespace Shared
{
    public class OrderUpdatedEvent
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public required string CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public required string ShippingAddress { get; set; }
    }
}
