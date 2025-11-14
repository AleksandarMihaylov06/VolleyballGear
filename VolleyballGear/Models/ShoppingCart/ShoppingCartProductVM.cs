using System.ComponentModel.DataAnnotations;

namespace VolleyballGear.Models.ShoppingCart
{
    public class ShoppingCartProductVM
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; } = null!;

        [Required]
        public string ProductPrice { get; set; } = null!; // Display price as string

        public string? Picture { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public bool HasDiscount { get; set; }

        // SUGGESTED ADDITIONS:
        public decimal PriceDecimal { get; set; } // For calculations
        public decimal OriginalPrice { get; set; } // Price before discount
        public string? Brand { get; set; } // Brand name
        public string? Category { get; set; } // Category name
        public int StockAvailable { get; set; } // To check if enough stock
        public decimal LineTotal => PriceDecimal * Quantity; // Total for this line item
    }
}
