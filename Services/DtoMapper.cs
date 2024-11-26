using AutoMapper;
using Core.Dtos;
using Core.Entities;

namespace Services;

public class DtoMapper : Profile
{
    public DtoMapper()
    {
        CreateMap<ProductDto, Product>().ReverseMap();
        CreateMap<AppUserDto, AppUser>().ReverseMap();
    }
}