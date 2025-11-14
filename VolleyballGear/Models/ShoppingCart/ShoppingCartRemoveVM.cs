using Microsoft.Build.Framework;

namespace VolleyballGear.Models.ShoppingCart
{
    public class ShoppingCartRemoveVM
    {
        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        public int ProductId { get; set; }
    }
}
