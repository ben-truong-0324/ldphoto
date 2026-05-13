using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Portfolio.Core.Models;
using Portfolio.Core.Services;

namespace PortfolioShop.Pages.Checkout
{
    public class ConfirmationModel : PageModel
    {
        private readonly OrderService _orderService;

        public ConfirmationModel(OrderService orderService)
        {
            _orderService = orderService;
        }

        public Order Order { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string orderNumber)
        {
            if (string.IsNullOrEmpty(orderNumber))
            {
                return RedirectToPage("/Index");
            }

            Order = await _orderService.GetOrderByNumberAsync(orderNumber);
            
            if (Order == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}