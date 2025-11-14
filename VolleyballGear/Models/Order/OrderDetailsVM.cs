using VolleyballGear.Data.Domain;

namespace VolleyballGear.Models.Order
{
    public class OrderDetailsVM
    {
        public int OrderId { get; set; }
        public string? UserEmail { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; } // ✅ use enum
        public decimal TotalPrice { get; set; }
        public List<OrderProductIndexVM> OrderProducts { get; set; } = new();

    }
}
