using AutoMapper;
using FruitSA.Model;

namespace FruitSA.API.Mappers
{
    public class Mapping:Profile
    {
        public Mapping()
        {
            CreateMap<Product, AddProductModel>().ReverseMap();
            //CreateMap<AddProductModel, Product>();
        }
    }
}
