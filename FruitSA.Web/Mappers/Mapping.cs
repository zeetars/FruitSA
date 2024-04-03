using AutoMapper;
using FruitSA.Model;

namespace FruitSA.Web.Models
{
    public class Mapping:Profile
    {
        public Mapping()
        {
            CreateMap<Product, AddProductModel>()
                .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<AddProductModel, Product>();

        }
    }
}
