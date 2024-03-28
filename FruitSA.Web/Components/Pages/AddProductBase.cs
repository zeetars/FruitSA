using AutoMapper;
using FruitSA.Model;
using FruitSA.Web.Models;
using FruitSA.Web.Services;
using Microsoft.AspNetCore.Components;

namespace FruitSA.Web.Components.Pages
{
    public class AddProductBase : ComponentBase
    {

        [Parameter]
        public string id { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }
        [Inject]
        public IProductService ProductService { get; set; }
        public Product Product { get; set; } = new Product();

        [Inject]
        public ICategoryService CategoryService { get; set; }
        public IEnumerable<Category> Categories { get; set; } = Enumerable.Empty<Category>();


        public AddProductModel AddProductModel { get; set; } = new AddProductModel();

        [Inject]
        public IMapper Mapper { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Categories = (await CategoryService.GetCategories()).ToList();

            Product = await ProductService.GetProductById(int.Parse(id));
        }

        protected async Task HandleValidSubmit()
        {

            int.TryParse(id, out int ProductId);


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
