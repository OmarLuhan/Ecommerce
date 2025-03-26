using AutoMapper;
using Ecommerce.api.Dto;
using Ecommerce.api.Model;

namespace Ecommerce.api.Helpers;

public class AutoMapperProfile:Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<User, SessionDto>();

        CreateMap<Category, CategoryDto>().ReverseMap();

        CreateMap<Product, ProductDto>().ForMember(dest=>dest.CategoryName,
            opt=>opt.MapFrom(src=>src.Category!.Name));
        CreateMap<ProductDto, Product>().ForMember(dest=>dest.CategoryId,
            opt=>opt.Ignore());
        
        CreateMap<SaleDetail,SaleDetailDto>().ReverseMap();
        CreateMap<Sale, SaleDto>().ReverseMap();
    }
}