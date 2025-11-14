using System.ComponentModel.DataAnnotations;

namespace VolleyballGear.Models.ShoppingCart
{
    public class ShoppingCartAddVM
    {
        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid product")]
        public int ProductId { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
        public int Quantity { get; set; } = 1; // Default to 1
    }
}
