using System.ComponentModel.DataAnnotations;

namespace VolleyballGear.Data.Domain
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public string UserId { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;
        public virtual IEnumerable<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
    }
}
