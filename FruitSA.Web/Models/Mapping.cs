using AutoMapper;
using FruitSA.Model;

namespace FruitSA.Web.Models
{
    public class Mapping:Profile
    {
        public Mapping()
        {
            CreateMap<Product, AddProductModel>();
            CreateMap<AddProductModel, Product>();
        }
    }
}
