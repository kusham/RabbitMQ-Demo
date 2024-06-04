using ProductOrders.API.Models;
using ProductOrders.API.Repository;
using Shared;

namespace ProductOrders.API.Serives
{
    public class OrderService : GenericService<Order>, IOrderService
    {
        public OrderService(IOrderRepository repository) : base(repository)
        {
        }
    }
}
