namespace VolleyballGear.Models.Order
{
    public class OrderProductIndexVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Pic { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; } // store numeric value, not formatted string
    }
}
