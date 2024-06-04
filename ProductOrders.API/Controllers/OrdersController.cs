using Microsoft.AspNetCore.Mvc;
using ProductOrders.API.IntegrationEvents;
using ProductOrders.API.Models;
using ProductOrders.API.Serives;
using Shared;

namespace ProductOrders.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrdersController> _logger;
        private readonly IEventBus _eventBus;

        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger, IEventBus eventBus)
        {
            this._orderService = orderService;
            this._logger = logger;
            this._eventBus = eventBus;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAll()
        {
            try
            {
                var orders = await _orderService.GetAllAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all orders.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }

        // GET: api/Orders/:id
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetById(Guid id)
        {
            try
            {
                var order = await _orderService.GetByIdAsync(id);
                if (order == null)
                {
                    _logger.LogWarning("Order with id {OrderId} not found.", id);
                    return NotFound();
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the order with id {OrderId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<Order>> Create(Order order)
        {
            try
            {
                await _orderService.CreateAsync(order);
                return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new order.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }

        // PUT: api/Orders/:id
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Order order)
        {
            if (id != order.Id)
            {
                _logger.LogWarning("Order ID mismatch: {OrderId} != {OrderIdBody}", id, order.Id);
                return BadRequest("Order ID mismatch.");
            }

            try
            {
                await _orderService.UpdateAsync(order);

                await _eventBus.PublishAsync(new OrderUpdatedEvent
                {
                    Id = order.Id,
                    Amount = order.Amount,
                    CustomerName = order.CustomerName,
                    CustomerEmail = order.CustomerEmail,
                    ShippingAddress = order.ShippingAddress,
                    //Status = order.Status,
                    CreatedAt = order.CreatedAt
                });
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the order with id {OrderId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }

        // DELETE: api/Orders/:id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var existingOrder = await _orderService.GetByIdAsync(id);
                if (existingOrder == null)
                {
                    _logger.LogWarning("Order with id {OrderId} not found.", id);
                    return NotFound();
                }

                await _orderService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the order with id {OrderId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }
    }
}
