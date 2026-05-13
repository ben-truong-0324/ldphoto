namespace Portfolio.Core.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        // --- The Final Customer Selections ---
        public ProductFormat Format { get; set; }
        public PrintFinish Finish { get; set; }
        public bool IsFramed { get; set; }

        public Order Order { get; set; }
        public string Size { get; set; }
    }
}