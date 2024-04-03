using System.ComponentModel.DataAnnotations;

namespace FruitSA.Web.Models
{
    public class AddProductModel
    {
        public int ProductId { get; set; }
        [Required]
        public string ProductCode { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public string CategoryName { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string Image { get; set; }
        
    }
}
