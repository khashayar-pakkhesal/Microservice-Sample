using AutoMapper;
using Basket.Api.Entites;
using EventBus.Messages.Events;

namespace Basket.Api.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BasketCheckout, BasketCheckoutEvent>().ReverseMap();
        }
    }
}
