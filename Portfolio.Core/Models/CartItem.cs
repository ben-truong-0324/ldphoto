namespace Portfolio.Core.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        
        // This price should reflect the calculated price of the specific variant
        public decimal Price { get; set; } 
        public int Quantity { get; set; }
        public string UserId { get; set; }

        // --- New Customer Selections ---
        public ProductFormat SelectedFormat { get; set; }
        public PrintFinish SelectedFinish { get; set; }
        public bool IsFramed { get; set; } 
        public string SelectedSize { get; set; }

        // -------------------------------

        public decimal Total => Price * Quantity;
    }
}