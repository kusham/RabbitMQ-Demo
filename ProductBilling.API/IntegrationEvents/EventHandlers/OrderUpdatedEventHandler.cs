using MassTransit;
using ProductBilling.API.Data;
using ProductBilling.API.Models;
using Shared;

namespace ProductBilling.API.IntegrationEvents.EventHandlers
{
    public sealed class OrderUpdatedEventHandler : IConsumer<OrderUpdatedEvent>
    {
        private readonly ILogger<OrderUpdatedEventHandler> _logger;
        private readonly ApplicationDbContext _context;

        public OrderUpdatedEventHandler(ILogger<OrderUpdatedEventHandler> logger, ApplicationDbContext applicationDbContext)
        {
            this._logger = logger;
            this._context = applicationDbContext;
        }

        public async Task Consume(ConsumeContext<OrderUpdatedEvent> context)
        {
            var message = context.Message;
            _logger.LogInformation("Order updated event received: {OrderUpdatedEvent} -------> Successfully recieved!", message);

            try
            {
                // Check if a billing record already exists for the given order Id
                var existingBilling = _context.Billings.FirstOrDefault(b => b.Id == message.Id);

                if (existingBilling != null)
                {
                    // Update existing billing record
                    existingBilling.Amount = message.Amount;
                    existingBilling.CustomerName = message.CustomerName;
                    existingBilling.CustomerEmail = message.CustomerEmail;
                    existingBilling.BillingAddress = message.ShippingAddress;
                    existingBilling.Notes = $"Updated on {DateTime.UtcNow}";

                    _context.Billings.Update(existingBilling);
                }
                else
                {
                    // Create new billing record
                    var newBilling = new Billing
                    {
                        Id = message.Id,
                        Amount = message.Amount,
                        CreatedAt = message.CreatedAt,
                        CustomerName = message.CustomerName,
                        CustomerEmail = message.CustomerEmail,
                        BillingAddress = message.ShippingAddress,
                    };

                    _context.Billings.Add(newBilling);
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation("Billing record for order : {OrderId} ----------> processed successfully", message.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing order updated event for order {OrderId}", message.Id);
                throw;
            }
        }
    }
}
