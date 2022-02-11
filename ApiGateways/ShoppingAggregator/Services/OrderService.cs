using ShoppingAggregator.Contracts;
using ShoppingAggregator.Extensions;
using ShoppingAggregator.Models;

namespace ShoppingAggregator.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(_httpClient));
        }

        public async Task<IEnumerable<OrderModel>> GetOrdersByUsername(string username)
        {
            var response = await _httpClient.GetAsync($"Api/v1/Order/{username}");

            return await response.ReadContentAs<List<OrderModel>>();
        }
    }
}
