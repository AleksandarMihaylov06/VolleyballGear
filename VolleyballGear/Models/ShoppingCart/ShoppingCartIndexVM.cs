using VolleyballGear.Data.Domain;
using System.ComponentModel.DataAnnotations;

namespace VolleyballGear.Models.ShoppingCart
{
    public class ShoppingCartIndexVM
    {
        [Required]
        public string UserId { get; set; } = null!;

        public List<ShoppingCartProductVM> Products { get; set; } = new List<ShoppingCartProductVM>();

        // Changed from nullable to always have a value
        public string TotalPrice { get; set; } = "$0.00";

        // SUGGESTED ADDITIONS:
        public decimal TotalPriceDecimal { get; set; } // For calculations
        public int TotalItems => Products?.Sum(p => p.Quantity) ?? 0;
        public bool IsEmpty => Products == null || !Products.Any();
    }
}
