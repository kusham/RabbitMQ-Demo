using ProductOrders.API.Data;
using ProductOrders.API.Models;
using Shared;

namespace ProductOrders.API.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
