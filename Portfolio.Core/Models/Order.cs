namespace Portfolio.Core.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string OrderNumber { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string PayPalOrderId { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerName { get; set; }
        public List<OrderItem> OrderItems { get; set; }

        public string ShippingAddress { get; set; } = string.Empty;
        public string ShippingCity { get; set; } = string.Empty;
        public string ShippingState { get; set; } = string.Empty;
        public string ShippingZip { get; set; } = string.Empty;
    }
}
