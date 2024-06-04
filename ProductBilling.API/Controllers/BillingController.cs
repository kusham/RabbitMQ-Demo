using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductBilling.API.Models;
using ProductBilling.API.Serives;

namespace ProductBilling.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillingController : ControllerBase
    {
        private readonly IBillingService _billingService;
        private readonly ILogger<BillingController> _logger;

        public BillingController(IBillingService billingService, ILogger<BillingController> logger)
        {
            this._billingService = billingService;
            this._logger = logger;
        }

        // GET: api/Billings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Billing>>> GetAll()
        {
            try
            {
                var billings = await _billingService.GetAllAsync();
                return Ok(billings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all billings.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }

        // GET: api/Billings/:id
        [HttpGet("{id}")]
        public async Task<ActionResult<Billing>> GetById(Guid id)
        {
            try
            {
                var billing = await _billingService.GetByIdAsync(id);
                if (billing == null)
                {
                    _logger.LogWarning("Billing with id {BillingId} not found.", id);
                    return NotFound();
                }

                return Ok(billing);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the billing with id {BillingId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }

        // POST: api/Billings
        [HttpPost]
        public async Task<ActionResult<Billing>> Create(Billing billing)
        {
            try
            {
                await _billingService.CreateAsync(billing);
                return CreatedAtAction(nameof(GetById), new { id = billing.Id }, billing);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new billing.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }

        // PUT: api/Billings/:id
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Billing billing)
        {
            if (id != billing.Id)
            {
                _logger.LogWarning("Billing ID mismatch: {BillingId} != {BillingIdBody}", id, billing.Id);
                return BadRequest("Billing ID mismatch.");
            }

            try
            {
                await _billingService.UpdateAsync(billing);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the billing with id {BillingId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }

        // DELETE: api/Billings/:id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var existingBilling = await _billingService.GetByIdAsync(id);
                if (existingBilling == null)
                {
                    _logger.LogWarning("Billing with id {BillingId} not found.", id);
                    return NotFound();
                }

                await _billingService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the billing with id {BillingId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }
    }
}
