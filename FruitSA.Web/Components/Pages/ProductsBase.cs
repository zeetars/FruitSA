using AutoMapper;
using FruitSA.Model;
using FruitSA.Web.Models;
using FruitSA.Web.Services;
using Microsoft.AspNetCore.Components;

namespace FruitSA.Web.Components.Pages
{
    public class ProductsBase:ComponentBase
    {
        //Objects for the Products List.
        [Inject]
        public IProductService ProductService { get; set; }
        public IEnumerable<Product> Products { get; set; }
        [Inject]
        public ICategoryService CategoryService { get; set; }
        public IEnumerable<Category> Categories { get; set; } = Enumerable.Empty<Category>();

        protected override async Task OnInitializedAsync()
        {
            Products = (await ProductService.GetProducts()).ToList();
            Categories = (await CategoryService.GetCategories()).ToList();
        }

    }
}
