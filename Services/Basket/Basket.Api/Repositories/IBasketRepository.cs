using Basket.Api.Entites;
using System.Threading.Tasks;


namespace Basket.Api.Repositories
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasketAsync(string username);

        Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket);

        Task DeleteBasketAsync(string username);
    }
}
