using OfficeOpenXml;
using FruitSA.Model;


namespace FruitSA.Web.Providers
{
    public class ExcelService
    {

        private readonly List<Product> Products;

        public ExcelService(List<Product> Products)
        {
            this.Products = Products;
            
        }

        public async Task<byte[]> GenerateExcelFileAsync()
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Products");

                // Adding headers
                worksheet.Cells[1, 1].Value = "ProductCode";
                worksheet.Cells[1, 2].Value = "Name";
                worksheet.Cells[1, 3].Value = "Description";
                worksheet.Cells[1, 4].Value = "Category";
                worksheet.Cells[1, 5].Value = "Price";
                worksheet.Cells[1, 6].Value = "CreatedDate";

                // Adding data rows
                for (int i = 0; i < Products.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = Products[i].ProductCode;
                    worksheet.Cells[i + 2, 2].Value = Products[i].Name;
                    worksheet.Cells[i + 2, 3].Value = Products[i].Description;
                    worksheet.Cells[i + 2, 4].Value = Products[i].Category.Name;
                    worksheet.Cells[i + 2, 5].Value = Products[i].Price;
                    worksheet.Cells[i + 2, 6].Value = Products[i].CreatedDate.ToString("yyyy-MM-dd");
                }

                // Convert package to bytes
                return package.GetAsByteArray();
            }
        }
    }

}

