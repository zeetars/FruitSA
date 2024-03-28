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

        public UniqueCodeValidator UniqueCodeValidator { get; set; }

        public string? errorMessage;
        protected override async Task OnInitializedAsync()
        {
            Categories = (await CategoryService.GetCategories()).ToList();

            int.TryParse(Id, out int CategoryId);
            if (CategoryId != 0)
            {
                Category = await CategoryService.GetCategoryById(CategoryId);
               
            }          

        }

        protected async Task HandleValidSubmit()
        {
            int.TryParse(Id, out int CategoryId);
            
            Category result = null;
            if (CategoryId != 0)
            {

                result = await CategoryService.UpdateCategory(Category);

            }
            else
            {
                errorMessage = "";
                var uniqueCode = Category.CategoryCode.ToUpper();
                if (uniqueCode != null)
                {
                    bool isValide;
                    var notAvailable = Categories.FirstOrDefault(c => c.CategoryCode == uniqueCode);
                    if (notAvailable == null)
                    {
                        isValide = UniqueCodeValidator.Validate(uniqueCode);
                        if (!isValide)
                        {
                            errorMessage = "Category Code is required in the format ABC123.";
                            return;
                        }

                        Category.CategoryCode = uniqueCode;
                    }
                    else 
                    {
                        errorMessage = "Category Code already assigned to another Category.";
                        return;
                    }
                }
                else
                {
                    errorMessage = "Category Code is required in the format ABC123.";
                    return;
                }

                result = await CategoryService.CreateCategory(Category);
            }

            if (result != null)
            {
                NavigationManager.NavigateTo("/categories");
            }
        }
       
    }
}
