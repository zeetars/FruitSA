using System.ComponentModel.DataAnnotations;

namespace FruitSA.Model
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string CategoryCode { get; set; }
        [Required]
        public byte IsActive { get; set; } = 1; 
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
