using ProductBilling.API.Models;
using ProductBilling.API.Repository;
using Shared;

namespace ProductBilling.API.Serives
{
    public class BillingService: GenericService<Billing>, IBillingService
    {
        public BillingService(IBillingRepository repository) : base(repository)
        {
        }
    }
}
