using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Portfolio.Core.Models;
using Portfolio.Core.Services;
using System.ComponentModel.DataAnnotations;

namespace PortfolioShop.Pages.Shop
{
    public class CheckoutModel : PageModel
    {
        private readonly CartService _cartService;
        private readonly OrderService _orderService;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService; // 1. Define the field

        // 2. Inject the service through the constructor
        public CheckoutModel(
            CartService cartService,
            OrderService orderService,
            IConfiguration configuration,
            EmailService emailService)
        {
            _cartService = cartService;
            _orderService = orderService;
            _configuration = configuration;
            _emailService = emailService;
        }

        public string PayPalClientId { get; set; } = string.Empty;
        public List<CartItem> CartItems { get; set; } = new();
        public decimal Total { get; set; }

        [BindProperty]
        public CheckoutInput Input { get; set; } = new();

        public class CheckoutInput
        {
            [Required(ErrorMessage = "Name is required"), Display(Name = "Full Name")]
            public string Name { get; set; } = string.Empty;

            [Required(ErrorMessage = "Email is required"), EmailAddress, Display(Name = "Email Address")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "Address is required"), Display(Name = "Street Address")]
            public string Address { get; set; } = string.Empty;

            [Required(ErrorMessage = "City is required")]
            public string City { get; set; } = string.Empty;

            [Required(ErrorMessage = "State is required")]
            public string State { get; set; } = string.Empty;

            [Required(ErrorMessage = "Zip Code is required"), Display(Name = "Zip Code")]
            public string ZipCode { get; set; } = string.Empty;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            PayPalClientId = _configuration["PayPal:ClientId"] ?? "";

            CartItems = await _cartService.GetCartItemsAsync();
            Total = await _cartService.GetCartTotalAsync();

            if (!CartItems.Any()) return RedirectToPage("/Shop/Index");
            return Page();
        }

        public async Task<IActionResult> OnPostPlaceOrderAsync(string payPalOrderId)
        {
            if (!ModelState.IsValid)
            {
                PayPalClientId = _configuration["PayPal:ClientId"] ?? "";
                CartItems = await _cartService.GetCartItemsAsync();
                Total = await _cartService.GetCartTotalAsync();
                return Page();
            }

            // Create the Order in your JSON storage
            var order = await _orderService.CreateOrderAsync(
                "session-user",
                payPalOrderId,
                Input.Email,
                Input.Name,
                Input.Address,
                Input.City,
                Input.State,
                Input.ZipCode
            );

            // 3. Send notifications before clearing the cart (or after, depending on preference)
            try
            {
                // Customer Receipt
                await _emailService.SendOrderConfirmationAsync(Input.Email, order);

                // Use the simple text/html version for you (admin)
                // await _emailService.SendEmailAsync(
                //     "ben.truong.0324@gmail.com", 
                //     "NEW ORDER: " + order.OrderNumber, 
                //     $"<h3>New Sale!</h3><p>{Input.Name} just bought ${order.TotalAmount:F2} worth of art.</p>"
                // );
                await _emailService.SendOrderConfirmationAsync("ben.truong.0324@gmail.com", order, isAdminCopy: true);
            }
            catch (Exception ex)
            {
                // In production, you'd log this but likely still complete the checkout 
                // so the customer isn't stuck on the payment page.
                System.Diagnostics.Debug.WriteLine($"Email failed: {ex.Message}");
            }

            // Cleanup: Clear cart
            await _cartService.ClearCartAsync();

            return RedirectToPage("/Shop/OrderConfirmation", new { orderNumber = order.OrderNumber });
        }
    }
}