using ShoppingAggregator.Models;

namespace ShoppingAggregator.Contracts
{
    public interface IBasketService
    {
        Task<BasketModel> GetBasket(string username); 
    }
}
