using System.ComponentModel.DataAnnotations;

namespace VolleyballGear.Models.Order
{
    public class OrderCreateVM
    {
        [Required]
        public decimal TotalPrice { get; set; }
        public virtual List<CreateOrderProductVM> OrderProducts { get; set; } = new List<CreateOrderProductVM>();
        public bool HasDiscount { get; set; }
    }
}
