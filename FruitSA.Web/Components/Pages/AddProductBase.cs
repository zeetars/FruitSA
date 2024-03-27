using AutoMapper;
using FruitSA.Model;
using FruitSA.Web.Models;
using FruitSA.Web.Services;
using Microsoft.AspNetCore.Components;

namespace FruitSA.Web.Components.Pages
{
    public class AddProductBase:ComponentBase
    {
      
        [Inject]
        NavigationManager NavigationManager { get; set; }
        [Inject]
        public IProductService ProductService { get; set; }
        [Inject]
        public ICategoryService CategoryService { get; set; }
        public IEnumerable<Category> Categories { get; set; } = Enumerable.Empty<Category>();

        [Parameter]
        public string id { get; set; }
        public Product Product { get; set; } = new Product();
        public AddProductModel AddProductModel { get; set; } = new AddProductModel();

        [Inject]
        public IMapper Mapper { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Categories = (await CategoryService.GetCategories()).ToList();
            int.TryParse(id, out int ProductId);
            if (ProductId != 0)
            {
                Product = await ProductService.GetProductById(ProductId);
                Mapper.Map(Product, AddProductModel);
            }
            

        }

        protected async Task HandleValidSubmit()
        {
            int.TryParse(id, out int ProductId);
           
            Mapper.Map(AddProductModel, Product);
            Product result = null;
            if (ProductId != 0)
            {

                result = await ProductService.UpdateProduct(Product);

            }
            else
            {
                result = await ProductService.CreateProduct(Product);
            }

            if (result != null)
            {
                NavigationManager.NavigateTo("/products");
            }
        }
    }
}
