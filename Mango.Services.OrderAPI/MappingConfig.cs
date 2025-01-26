using AutoMapper;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Models.Dto;

namespace Mango.Services.OrderAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CartHeaderDto, OrderHeaderDto>()
                      .ForMember(oh => oh.OrderTotal, u => u.MapFrom(ch => ch.CartTotal)).ReverseMap();

                config.CreateMap<CartDetailsDto, OrderDetailsDto>()
                      .ForMember(cd => cd.ProductName, u => u.MapFrom(od => od.Product.Name))
                      .ForMember(cd => cd.Price, u => u.MapFrom(od => od.Product.Price));

                config.CreateMap<OrderDetailsDto, CartDetailsDto>();

                config.CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();
                config.CreateMap<OrderDetails, OrderDetailsDto>().ReverseMap();

            });

            return mappingConfig;
        }
    }
}
