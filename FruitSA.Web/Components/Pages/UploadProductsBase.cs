using AutoMapper;
using FruitSA.Model;
using FruitSA.Web.Models;
using FruitSA.Web.Providers;
using FruitSA.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace FruitSA.Web.Components.Pages
{
    public class UploadProductsBase:ComponentBase
    {
       
        
        [Inject]
        NavigationManager NavigationManager { get; set; }
        [Inject]
        public IProductService ProductService { get; set; }
        public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();

        UniqueCodeGenerator UniqueCodeGenerator { get; set; }

        private IBrowserFile? selectedFile;
        public string? errorMessage;
        public string? successMessage;

        protected async Task UploadProducts()
        {        

            errorMessage = "";
            successMessage = "";
            if (selectedFile != null)
            {
                try
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await selectedFile.OpenReadStream().CopyToAsync(memoryStream);
                        memoryStream.Position = 0;

                        using (var package = new OfficeOpenXml.ExcelPackage(memoryStream))
                        {
                            var worksheet = package.Workbook.Worksheets[0]; // Assuming the data is in the first worksheet

                            var rowCount = worksheet.Dimension.Rows;
                            var products = new List<Product>();

                            for (int row = 2; row <= rowCount; row++) // Assuming the first row contains headers
                            {
                                var product = new Product
                                {
                                    ProductCode = worksheet.Cells[row, 1].Value.ToString(),
                                    Name = worksheet.Cells[row, 2].Value.ToString(),
                                    Description = worksheet.Cells[row, 3].Value.ToString(),
                                    CategoryId = Convert.ToInt32(worksheet.Cells[row, 4].Value),
                                    Price = Convert.ToDecimal(worksheet.Cells[row, 5].Value),
                                    Image = "default-image.jpg"
                                };

                                products.Add(product);
                            }
                            
                            // Add products to the database
                            Products = await ProductService.CreateProducts(products);
                        }
                    }

                    successMessage = "Products uploaded successfully.";
                    return;
                }
                catch (Exception ex)
                {
                    errorMessage = $"Error uploading products: {ex.Message}";
                    return;
                }
            }
            else
            {
                errorMessage = "Please select a file.";
                return;
            }

            //if(successMessage != "")
            //{
            //    NavigationManager.NavigateTo("/products");
            //}

            //return;
            
        }
      

        public void HandleFileChange(InputFileChangeEventArgs e)
        {
            selectedFile = e.File;
        }

    }
}
