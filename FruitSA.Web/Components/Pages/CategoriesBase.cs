using FruitSA.Model;
using FruitSA.Web.Providers;
using FruitSA.Web.Services;
using Microsoft.AspNetCore.Components;

namespace FruitSA.Web.Components.Pages
{
    public class CategoriesBase: ComponentBase
    {
        [Parameter]
        public int CategoryId { get; set; }
        [Parameter]
        public string Token { get; set; } = "";
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
            errorMessage = "";
            try
            {
                Categories = (await CategoryService.GetCategories(Token)).ToList();

                if (CategoryId != 0)
                {
                    Category = await CategoryService.GetCategoryById(Token, CategoryId);

                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
        }

        //Add a Category and Validate the uniqueCode to meet the RegEx Partten @"^[A-Za-z]{3}\d{3}$"
        
        protected async Task HandleValidSubmit()
        {
                        
            Category result = null;
            errorMessage = "";
            var uniqueCode = Category.CategoryCode.ToUpper();
            if (CategoryId == 0)
            {
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
                        if (Category.CategoryId != 0)
                        {
                            isValide = UniqueCodeValidator.Validate(uniqueCode);
                            if (!isValide)
                            {
                                errorMessage = "Category Code is required in the format ABC123.";
                                return;
                            }

                            Category.CategoryCode = uniqueCode;
                        }
                        errorMessage = "Category Code already assigned to another Category.";
                        return;
                    }
                }
                else
                {
                    errorMessage = "Category Code is required in the format ABC123.";
                    return;
                }
            }

            try
            {
                if (CategoryId != 0)
                {
                    result = await CategoryService.UpdateCategory(Token, Category);
                }
                else
                {
                    result = await CategoryService.CreateCategory(Token, Category);                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }            
            if (result != null)
            {
                NavigationManager.NavigateTo($"/categories/{Token}");
            }
        }
       
    }
}
