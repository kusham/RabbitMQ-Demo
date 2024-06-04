using ProductBilling.API.Data;
using ProductBilling.API.Models;
using Shared;

namespace ProductBilling.API.Repository
{
    public class BillingRepository : GenericRepository<Billing>, IBillingRepository
    {
        public BillingRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
