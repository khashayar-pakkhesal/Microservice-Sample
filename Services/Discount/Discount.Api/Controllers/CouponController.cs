using Discount.Api.Entites;
using Discount.Api.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Discount.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CouponController : ControllerBase
    {
        private readonly IDiscountRepository _repository;

        public CouponController(IDiscountRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("{productName}")]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> GetDiscocunt(string productName)
        {
            return Ok(await _repository.GetDiscountAsync(productName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CreateDiscocunt([FromBody] Coupon coupon)
        {
            await _repository.CreateDiscountAsync(coupon);

            return CreatedAtRoute("GetDiscount", coupon);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateDiscocunt([FromBody] Coupon coupon)
        {
            return Ok(await _repository.UpdateDiscountAsync(coupon));
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteDiscocunt(int id)
        {
            return Ok(await _repository.DeleteCouponAsync(id));
        }
    }
}
