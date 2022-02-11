using ShoppingAggregator.Models;

namespace ShoppingAggregator.Contracts
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderModel>> GetOrdersByUsername(string username);
    }
}
