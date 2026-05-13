using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Portfolio.Core.Models;
using Portfolio.Core.Services;

namespace PortfolioShop.Pages.Shop
{
    public class OrderConfirmationModel : PageModel
    {
        private readonly OrderService _orderService;

        public OrderConfirmationModel(OrderService orderService)
        {
            _orderService = orderService;
        }

        public Order? Order { get; set; }

        public async Task<IActionResult> OnGetAsync(string orderNumber)
        {
            if (string.IsNullOrEmpty(orderNumber))
            {
                return RedirectToPage("/Index");
            }

            // Fetch the order details from the JSON file to display on the page
            Order = await _orderService.GetOrderByNumberAsync(orderNumber);

            if (Order == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}