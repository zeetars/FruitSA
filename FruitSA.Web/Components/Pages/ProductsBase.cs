using AutoMapper;
using FruitSA.Model;
using FruitSA.Web.Models;
using FruitSA.Web.Providers;
using FruitSA.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace FruitSA.Web.Components.Pages
{
    public class ProductsBase:ComponentBase
    {
        [Parameter]
        public int ProductId { get; set; }
        [Parameter]
        public string Token { get; set; } = "";
        public string? CategoryName { get; set; }

        [Inject]
        IJSRuntime JSRuntime { get; set; }
      
        [Inject]
        NavigationManager? NavigationManager { get; set; }
        [Inject]
        public IProductService? ProductService { get; set; }
        [Inject]
        public IMapper Mapper { get; set; }
        public Product Product { get; set; } = new Product();
        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        public AddProductModel ProductModel { get; set; } = new AddProductModel();

        [Inject]
        public ICategoryService? CategoryService { get; set; }
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();

        protected ConfirmationBase Confirmation {  get; set; }
        
        UniqueCodeGenerator? UniqueCodeGenerator { get; set; }
        
        //Image Upload data fields
        private IBrowserFile? selectedFile;
        public string? ErrorMessage;
        public string? PriceErrorMessage;
        public string? ProductErrorMessage;
        private long maxFileSize = 1024 * 1024 * 3;

        //Pagination Data fields
        public int currentPage = 1;
        public int totalPages = 1;
        public int pageSize = 10;

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(Token))
            {
                ErrorMessage = "";
                try
                {
                    //Loading Paginated Products  
                    await LoadData();
                    Categories = (await CategoryService.GetCategories(Token)).ToList();
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Error retrieving Data: {@ex.Message}";
                    return;
                }

                //If an Id is send via GET retrieve the product
                //else generate a uniqueCode before display the AddProduct form.
                
                if (ProductId != 0)
                {
                    Product = await ProductService.GetProductById(Token, ProductId);
                    Mapper.Map(Product, ProductModel);
                    CategoryName = Product.Category.Name;
                }
                else
                {

                    string uniqueCode = UniqueCodeGenerator.GenerateUniqueCode();
                    if (uniqueCode != null)
                    {
                        var codeTaken = await ProductService.GetProductByCode(Token, uniqueCode);
                        while (codeTaken)
                        {
                            uniqueCode = UniqueCodeGenerator.GenerateUniqueCode();
                            codeTaken = await ProductService.GetProductByCode(Token, uniqueCode);
                        }
                    }

                    ProductModel.ProductCode = uniqueCode;

                }
            }

        }

        //GetProducts By set totalPages and pageSize
        private async Task LoadData()
        {
            Products = await ProductService.GetProducts(Token, currentPage, pageSize);
            totalPages = (int)Math.Ceiling((double)await ProductService.GetProductCount(Token) / pageSize);
        }

        //Creating a New Product If Id = 0
        //Else Update the product.
        //Handling ProductImage Upload (Can be utilized as a seperate Provider/Service class)
        protected async Task HandleValidSubmit()
        {
            
            Mapper.Map(ProductModel, Product);
            Product.CategoryId = Categories.FirstOrDefault(c => c.Name == ProductModel.CategoryName).CategoryId;
            ErrorMessage = "";
            ProductErrorMessage = "";
            PriceErrorMessage = "";
            if (Product.CategoryId == 0)
            {
                ProductErrorMessage = "Select the Product Category.";
                return;
            }
            if (Product.Price == 0)
            {
                PriceErrorMessage = "The Product Price is required.";
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

                result = await ProductService.UpdateProduct(Token, Product);

            }
            else
            {
               
                result = await ProductService.CreateProduct(Token, Product);
            }

            if (result != null)
            {
                NavigationManager.NavigateTo($"/products/{Token}");
            }
        }

        //Deleting a Product
        protected void HandleProductDelete()
        {
            Confirmation.Show();
        }

        protected async Task ConfirmDelete_CLick(bool deleteConfirmed)
        {
            if (deleteConfirmed)
            {
                var result = await ProductService.DeleteProduct(Token, ProductId);

                if (result != null)
                {
                    NavigationManager.NavigateTo($"/products/{Token}");
                }
            }
        }

        protected void HandleDownloadProducts()
        {
            Confirmation.Show();
        }

        //Download Products to Excel use the ExcelService class under Providers folder 
        //OfficeOpenXml
        protected async Task ConfirmDownload_CLick(bool downloadConfirmed)
        {
            if (downloadConfirmed)
            {
                ErrorMessage = "";
                try
                {
                    ExcelService excelService = new ExcelService(Products.ToList());

                    byte[] excelFile = await excelService.GenerateExcelFileAsync();

                    string fileName = "products.xlsx";

                    //wwwroot / js / downloads.js
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
            
        }

        //Handling Pagination, Next and Previous Pages
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
