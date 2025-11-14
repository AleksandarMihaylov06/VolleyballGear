using System.ComponentModel.DataAnnotations;

namespace VolleyballGear.Data.Domain
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string ProductName { get; set; } = null!;

        [Required]
        public int BrandId { get; set; }
        public virtual Brand Brand { get; set; } = null!;
        [Required]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;

        public string Picture { get; set; } = null!;

        [Range(0, 5000)]
        public int Quantity { get; set; }

        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public virtual IEnumerable<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    }
}
