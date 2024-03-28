using FruitSA.Model;
using FruitSA.Web.Providers;
using FruitSA.Web.Services;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;

namespace FruitSA.Web.Components.Pages
{
    public class CategoriesBase: ComponentBase
    {
        [Parameter]
        public string? Id { get; set; }      

        [Inject]
        NavigationManager? NavigationManager { get; set; }     

        [Inject]
        public ICategoryService? CategoryService { get; set; }
        public Category Category { get; set; } = new Category();
        public IEnumerable<Category> Categories { get; set; } = Enumerable.Empty<Category>();

        protected override async Task OnInitializedAsync()
        {
            Categories = (await CategoryService.GetCategories()).ToList();

            int.TryParse(Id, out int CategoryId);
            if (CategoryId != 0)
            {
                Category = await CategoryService.GetCategoryById(CategoryId);
               
            }          

        }
    }
}
