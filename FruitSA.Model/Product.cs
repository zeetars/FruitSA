using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FruitSA.Model
{
    public class Product
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public string ProductCode { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        [ForeignKey("nameof(CategoryKey)")]
        public int CategoryId { get; set; }       
        [Required]
        public decimal Price { get; set; }
        public string? Image { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
