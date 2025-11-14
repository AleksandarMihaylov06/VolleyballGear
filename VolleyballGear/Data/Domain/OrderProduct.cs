using System.ComponentModel.DataAnnotations;

namespace VolleyballGear.Data.Domain
{
    public class OrderProduct
    {
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; } = null!;
        [Required]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
    }
}