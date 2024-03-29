using AutoMapper;
using FruitSA.Model;
using FruitSA.Web.Models;
using FruitSA.Web.Providers;
using FruitSA.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.IO;

namespace FruitSA.Web.Components.Pages
{
    public class ProductsBase:ComponentBase
    {
        [Parameter]
        public string? Id { get; set; }
        public string? CategoryName { get; set; }

        [Inject]
        IJSRuntime JSRuntime { get; set; }
        [Inject]
        NavigationManager? NavigationManager { get; set; }
        [Inject]
        public IProductService? ProductService { get; set; }
        public Product Product { get; set; } = new Product();
        public IEnumerable<Product> Products { get; set; } = new List<Product>();

        [Inject]
        public ICategoryService? CategoryService { get; set; }
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();

        UniqueCodeGenerator? UniqueCodeGenerator { get; set; }
        
        //Image Upload data fields
        private IBrowserFile? selectedFile;
        public string? ErrorMessage;
        public string? ProductErrorMessage;
        private long maxFileSize = 1024 * 1024 * 3;

        //Pagination Data fields
        public int currentPage = 1;
        public int totalPages = 1;
        public int pageSize = 10;

        protected override async Task OnInitializedAsync()
        {

            await LoadData(); //Loading Paginated Products
            //Products = (await ProductService.GetProducts()).ToList();
            Categories = (await CategoryService.GetCategories()).ToList();

            int.TryParse(Id, out int ProductId);
            if (ProductId != 0)
            {
                Product = await ProductService.GetProductById(ProductId);
                CategoryName = Product.Category.Name;
            }
            else
            {
               var uniqueCode = UniqueCodeGenerator.GenerateUniqueCode();    
               if (uniqueCode != null) 
               {
                    var notAvailable = Products.FirstOrDefault(p => p.ProductCode == uniqueCode);
                    while(notAvailable != null) 
                    {
                        uniqueCode = UniqueCodeGenerator.GenerateUniqueCode();
                        notAvailable = Products.FirstOrDefault(p => p.ProductCode == uniqueCode);
                    }
               }

                Product.ProductCode = uniqueCode;

            }

        }

        private async Task LoadData()
        {
            Products = await ProductService.GetProducts(currentPage, pageSize);
            totalPages = (int)Math.Ceiling((double)await ProductService.GetProductCount() / pageSize);
        }

        //Creating a New Product when Id = 0 or Update when Id is > 0, Handling ProductImage Upload
        protected async Task HandleValidSubmit()
        {
            int.TryParse(Id, out int ProductId);

            ErrorMessage = "";
            ProductErrorMessage = "";
            if (Product.CategoryId == 0)
            {
                ProductErrorMessage = "Select the Product Category.";
                return;
            }
            if (selectedFile != null)
            {
                if (selectedFile.Size > maxFileSize)
                {
                    ErrorMessage = "File size exceeds the limit (3MB).";
                    return;
                }

                var extension = Path.GetExtension(selectedFile.Name).ToLower();
                if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                {
                    ErrorMessage = "Only JPG, JPEG and PNG files are allowed.";
                    return;
                }

                try
                {
                    var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    FileInfo fi = new FileInfo(selectedFile.Name);

                    var fileName = Path.GetRandomFileName();
                    fileName += fi.Extension;
                    var filePath = Path.Combine(uploadDir, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await selectedFile.OpenReadStream().CopyToAsync(fileStream);
                    }

                    Product.Image = fileName;
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                    return;
                }
            }
            else if(ProductId==0)
            {
                Product.Image = "default-image.jpg";
            }


            Product result = null;
            if (ProductId != 0)
            {

                result = await ProductService.UpdateProduct(Product);

            }
            else
            {
                //Product.ProductCode = uniqueProductCode;
                result = await ProductService.CreateProduct(Product);
            }

            if (result != null)
            {
                NavigationManager.NavigateTo("/products");
            }
        }

        //Deleting a Product
        protected async Task HandleProductDelete()
        {
            var result = await ProductService.DeleteProduct(int.Parse(Id));

            if (result != null)
            {
                NavigationManager.NavigateTo("/products");
            }
        }

        //Download Products to Excel use the ExcelService class under Providers folder 
        //OfficeOpenXml
        protected async Task HandleDownloadProducts()
        {
            ErrorMessage = "";
            try
            {
                ExcelService excelService = new ExcelService(Products.ToList());

                byte[] excelFile = await excelService.GenerateExcelFileAsync();

                string fileName = "products.xlsx";

                //wwwroot / js / downloadFile.js
                await Task.Run(async () =>
                {
                    await JSRuntime.InvokeVoidAsync("DownloadExcelFile", fileName, excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

                });
            }
            catch (Exception)
            {
                ErrorMessage = "Failed to download the Products, please contact support or try again later.";
                return;
            }
        }

        //Handling Pagination
        public void HandleFileChange(InputFileChangeEventArgs e)
        {
            selectedFile = e.File;
        }

        protected async Task NextPage()
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                await LoadData();
            }
        }

        protected async Task PreviousPage()
        {
            if (currentPage > 1)
            {
                currentPage--;
                await LoadData();
            }
        }

    }
}
