using VolleyballGear.Data.Domain;
using System.ComponentModel.DataAnnotations;

namespace VolleyballGear.Models.Order
{
    public class OrderIndexVM
    {
        public int OrderId { get; set; }
        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string? UserEmail { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; } // ✅ enum, not string
        public List<OrderProductIndexVM> OrderProducts { get; set; } = new();
        public decimal TotalPrice { get; set; }
    }
}
