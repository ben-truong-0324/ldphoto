using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Portfolio.Core.Services;
using System.Security.Claims;

namespace PortfolioShop.Pages.Checkout
{
    public class CheckoutIndexModel : PageModel
    {
        private readonly CartService _cartService;
        private readonly PayPalService _payPalService;
        private readonly EmailService _emailService;
        private readonly OrderService _orderService;
        private readonly IConfiguration _configuration;

        public CheckoutIndexModel(
            CartService cartService, 
            PayPalService payPalService, 
            EmailService emailService,
            OrderService orderService,
            IConfiguration configuration)
        {
            _cartService = cartService;
            _payPalService = payPalService;
            _emailService = emailService;
            _orderService = orderService;
            _configuration = configuration;
        }

        public decimal Total { get; set; }
        public string PayPalClientId { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            Total = await _cartService.GetCartTotalAsync();
            PayPalClientId = _configuration["PayPal:ClientId"] ?? string.Empty;
            
            // Get user info if authenticated
            if (User.Identity?.IsAuthenticated == true)
            {
                CustomerEmail = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
                CustomerName = User.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
            }
        }

        public async Task<IActionResult> OnPostCreateOrderAsync()
        {
            try
            {
                var total = await _cartService.GetCartTotalAsync();
                var order = await _payPalService.CreateOrder(total);
                return new JsonResult(new { id = order.Id });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message }) { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> OnPostCaptureOrderAsync(
            string orderId,
            string email,
            string name,
            string address,  
            string city,     
            string state,   
            string zipCode)  
        {
            try
            {
                var capture = await _payPalService.CaptureOrder(orderId);
                
                if (capture.Status == "COMPLETED")
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "anonymous";
                    
                    // Create order in database
                    var order = await _orderService.CreateOrderAsync(userId, orderId, email, name, address,
                                                                                        city,
                                                                                        state,
                                                                                        zipCode);
                    
                    // Send confirmation email
                    await _emailService.SendOrderConfirmationAsync(email, order);
                    
                    // Clear cart
                    await _cartService.ClearCartAsync();
                    
                    return new JsonResult(new { success = true, orderNumber = order.OrderNumber });
                }
                
                return new JsonResult(new { success = false, error = "Payment not completed" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, error = ex.Message }) { StatusCode = 500 };
            }
        }
    }
}
