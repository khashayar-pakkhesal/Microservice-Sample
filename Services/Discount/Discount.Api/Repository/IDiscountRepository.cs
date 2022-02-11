using Discount.Api.Entites;
using System.Threading.Tasks;

namespace Discount.Api.Repository
{
    public interface IDiscountRepository
    {
        Task<Coupon> GetDiscountAsync(string ProductName);

        Task<bool> CreateDiscountAsync(Coupon coupon);

        Task<bool> UpdateDiscountAsync(Coupon coupon);

        Task<bool> DeleteCouponAsync(int Id);


    }
}
