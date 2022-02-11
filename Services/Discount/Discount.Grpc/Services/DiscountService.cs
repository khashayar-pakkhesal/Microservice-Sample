using AutoMapper;
using Discount.Grpc.Entites;
using Discount.Grpc.Protos;
using Discount.Grpc.Repository;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using static Discount.Grpc.Protos.DiscountProtoService;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoServiceBase
    {
        private readonly IDiscountRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DiscountService> _logger;

        public DiscountService(IDiscountRepository repository, ILogger<DiscountService> logger, IMapper mapper = null)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _repository.GetDiscountAsync(request.ProductName);

            if (coupon is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not Found."));
            }
            _logger.LogInformation($"Discount is retireved for Product={coupon.ProductName}. the amount is {coupon.Amount}");
            return _mapper.Map<CouponModel>(coupon);
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);

            await _repository.CreateDiscountAsync(coupon);

            _logger.LogInformation("Discount is Successfully craeted. ProductName : {ProductName}", coupon.ProductName);

            return _mapper.Map<CouponModel>(coupon);
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);

            await _repository.UpdateDiscountAsync(coupon);

            _logger.LogInformation("Discount is Successfully Updated. ProductName : {ProductName}", coupon.ProductName);

            return _mapper.Map<CouponModel>(coupon);
        }


        public override async Task<DeleteResponseModel> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {

           var result =  await _repository.DeleteCouponAsync(request.Id);

            return new DeleteResponseModel
            {
                Succes = result
            };
        }
    }
}
