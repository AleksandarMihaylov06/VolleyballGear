namespace VolleyballGear.Models.Order
{
    public class CreateOrderProductVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? Pic { get; set; }
        public int Quantity { get; set; }
        public string Price { get; set; }
        public decimal Discount { get; set; }
    }
}
