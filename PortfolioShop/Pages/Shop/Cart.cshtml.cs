using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Portfolio.Core.Models;
using Portfolio.Core.Services;

namespace PortfolioShop.Pages.Cart
{
    public class CartIndexModel : PageModel
    {
        private readonly CartService _cartService;

        public CartIndexModel(CartService cartService)
        {
            _cartService = cartService;
        }

        public List<CartItem> CartItems { get; set; } = new();
        public decimal Total { get; set; }

        public async Task OnGetAsync()
        {
            CartItems = await _cartService.GetCartItemsAsync();
            Total = await _cartService.GetCartTotalAsync();
        }

        public async Task<IActionResult> OnPostRemoveAsync(int cartItemId)
        {
            await _cartService.RemoveFromCartAsync(cartItemId);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateQuantityAsync(int cartItemId, int quantity)
        {
            await _cartService.UpdateQuantityAsync(cartItemId, quantity);
            return RedirectToPage();
        }


        public async Task<IActionResult> OnPostAddToCartAsync(string productId, string productName, decimal price, int quantity = 1)
        {
            // Wrap the parameters into the new CartItem object
            var newItem = new CartItem
            {
                ProductId = productId,
                ProductName = productName,
                Price = price,
                Quantity = quantity,
                UserId = "session-user", // Keep this consistent with how you handle users

                // Since they are adding directly from the Index page, 
                // you have to provide default selections for the physical product:
                SelectedFormat = global::Portfolio.Core.Models.ProductFormat.StandardPrint,
                SelectedFinish = global::Portfolio.Core.Models.PrintFinish.None,
                IsFramed = false
            };

            // Pass the single object to the updated method
            await _cartService.AddToCartAsync(newItem);

            return RedirectToPage();
        }
    }
}