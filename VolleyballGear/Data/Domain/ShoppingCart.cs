using System.ComponentModel.DataAnnotations;

namespace VolleyballGear.Data.Domain
{
    public class ShoppingCart
    {
        [Required]
        public string UserId { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;
        [Required]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
