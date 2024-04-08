﻿using FruitSA.Model;
using FruitSA.Web.Providers;
using FruitSA.Web.Services;
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
            errorMessage = "";
            try
            {
                Categories = (await CategoryService.GetCategories(LoginBase.authToken)).ToList();

                int.TryParse(Id, out int CategoryId);
                if (CategoryId != 0)
                {
                    Category = await CategoryService.GetCategoryById(LoginBase.authToken, CategoryId);

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
            int.TryParse(Id, out int CategoryId);            
            Category result = null;
            errorMessage = "";
            var uniqueCode = Category.CategoryCode.ToUpper();
            if (Id == null)
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
                    result = await CategoryService.UpdateCategory(LoginBase.authToken, Category);
                }
                else
                {
                    result = await CategoryService.CreateCategory(LoginBase.authToken, Category);                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }            
            if (result != null)
            {
                NavigationManager.NavigateTo("/categories");
            }
        }
       
    }
}
