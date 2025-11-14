using System.ComponentModel.DataAnnotations;

namespace VolleyballGear.Models.ShoppingCart
{
    public class ShoppingCartUpdateVM
    {
        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Quantity must be between 0 and 100")]
        public int NewQuantity { get; set; }
    }
}
