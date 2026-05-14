using Portfolio.Core.Models;

namespace Portfolio.Core.Services
{
    public class OrderService
    {
        private readonly JsonFileDataService _fileService;
        private readonly CartService _cartService;
        private const string OrdersFile = "data/orders.json";

        public OrderService(JsonFileDataService fileService, CartService cartService)
        {
            _fileService = fileService;
            _cartService = cartService;
        }
        public async Task<Order> CreateOrderAsync(
                string userId, 
                string payPalOrderId, 
                string customerEmail, 
                string customerName,
                string shippingAddress,
                string shippingCity,
                string shippingState,
                string shippingZip)
        {
            var cartItems = await _cartService.GetCartItemsAsync();
            var totalAmount = await _cartService.GetCartTotalAsync();

            // 1. Fetch existing orders from JSON
            var allOrders = await _fileService.ReadAsync<Order>(OrdersFile);

            var order = new Order
            {
                Id = allOrders.Any() ? allOrders.Max(o => o.Id) + 1 : 1,
                UserId = userId,
                OrderNumber = GenerateOrderNumber(),
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                Status = "Completed",
                PayPalOrderId = payPalOrderId,
                CustomerEmail = customerEmail,
                CustomerName = customerName,
                ShippingAddress = shippingAddress,
                ShippingCity = shippingCity,
                ShippingState = shippingState,
                ShippingZip = shippingZip,
                OrderItems = cartItems.Select(cartItem => new OrderItem
                {
                    ProductId = cartItem.ProductId,
                    ProductName = cartItem.ProductName,
                    Price = cartItem.Price,
                    Quantity = cartItem.Quantity,
                    
                    // 1. Make sure the image carries over!
                    ImageUrl = cartItem.ImageUrl, 

                    // 2. Map the new physical options over!
                    Format = cartItem.SelectedFormat,
                    Size = cartItem.SelectedSize, 
                    Finish = cartItem.SelectedFinish,
                    IsFramed = cartItem.IsFramed

                }).ToList(),
            };

            // 2. Append new order and save back to disk
            allOrders.Add(order);
            await _fileService.WriteAsync(allOrders, OrdersFile);

            return order;
        }

        public async Task<Order?> GetOrderByNumberAsync(string orderNumber)
        {
            var allOrders = await _fileService.ReadAsync<Order>(OrdersFile);
            return allOrders.FirstOrDefault(o => o.OrderNumber == orderNumber);
        }

        public async Task<List<Order>> GetUserOrdersAsync(string userId)
        {
            var allOrders = await _fileService.ReadAsync<Order>(OrdersFile);
            return allOrders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToList();
        }

        private string GenerateOrderNumber()
        {
            return $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
}