using Microsoft.AspNetCore.Http;
using Portfolio.Core.Models;
using System.Security.Claims;

namespace Portfolio.Core.Services
{
    public class CartService
    {
        private readonly JsonFileDataService _fileService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CartFile = "data/cart_storage.json"; // Define the file path here

        public CartService(JsonFileDataService fileService, IHttpContextAccessor httpContextAccessor)
        {
            _fileService = fileService;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetUserId()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null) return "anonymous";

            // 1. If the user is logged in, use their ID
            if (context.User?.Identity?.IsAuthenticated == true)
            {
                return context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "anonymous";
            }

            // 2. Force session establishment if it's new
            // This ensures Session.Id is generated and available
            if (string.IsNullOrEmpty(context.Session.GetString("SessionKey")))
            {
                context.Session.SetString("SessionKey", Guid.NewGuid().ToString());
            }

            return context.Session.Id;
        }

        public async Task<List<CartItem>> GetCartItemsAsync()
        {
            // Pass the CartFile path to the ReadAsync method
            var allItems = await _fileService.ReadAsync<CartItem>(CartFile);
            var userId = GetUserId();
            return allItems.Where(c => c.UserId == userId).ToList();
        }

        public async Task AddToCartAsync(CartItem newItem)
        {
            // 1. Read the existing cart file
            var allItems = await _fileService.ReadAsync<CartItem>(CartFile) ?? new List<CartItem>();
            
            // 2. Enforce the current User ID
            newItem.UserId = GetUserId();

            // 3. Fallback for the image if it wasn't provided
            if (string.IsNullOrWhiteSpace(newItem.ImageUrl))
            {
                newItem.ImageUrl = "https://www.shutterstock.com/image-photo/sad-cute-brown-dog-look-260nw-2478688715.jpg";
            }

            // 4. Check if this EXACT variant is already in the cart for this user
            var existingItem = allItems.FirstOrDefault(c => 
                c.ProductId == newItem.ProductId && 
                c.UserId == newItem.UserId &&
                c.SelectedFormat == newItem.SelectedFormat &&
                c.SelectedSize == newItem.SelectedSize &&
                c.SelectedFinish == newItem.SelectedFinish &&
                c.IsFramed == newItem.IsFramed);

            if (existingItem != null)
            {
                // Update quantity if the exact variant already exists
                existingItem.Quantity += newItem.Quantity;
            }
            else
            {
                // 5. Generate a unique ID for the new entry (Incrementing integer)
                newItem.Id = allItems.Any() ? allItems.Max(x => x.Id) + 1 : 1;

                // 6. Add the completely formed item to the list
                allItems.Add(newItem);
            }

            // 7. PERSIST: Write the updated list back to the JSON file
            // Without this line, the cart will always appear empty on the next page load
            await _fileService.WriteAsync<CartItem>(allItems, CartFile);
        }

        public async Task ClearCartAsync()
        {
            // Pass the CartFile path to the ReadAsync method
            var allItems = await _fileService.ReadAsync<CartItem>(CartFile);
            var userId = GetUserId();
            allItems.RemoveAll(i => i.UserId == userId);

            // Pass the CartFile path to the WriteAsync method
            await _fileService.WriteAsync(allItems, CartFile);
        }

        public async Task UpdateQuantityAsync(int cartItemId, int newQuantity)
        {
            var allItems = await _fileService.ReadAsync<CartItem>(CartFile);
            var userId = GetUserId();
            var itemToUpdate = allItems.FirstOrDefault(i => i.Id == cartItemId && i.UserId == userId);

            if (itemToUpdate != null)
            {
                if (newQuantity > 0)
                {
                    itemToUpdate.Quantity = newQuantity;
                }
                else
                {
                    allItems.Remove(itemToUpdate); // Remove if they set quantity to 0
                }
                await _fileService.WriteAsync(allItems, CartFile);
            }
        }

        public async Task<int> GetCartCountAsync() => (await GetCartItemsAsync()).Sum(i => i.Quantity);
        public async Task<decimal> GetCartTotalAsync() => (await GetCartItemsAsync()).Sum(i => i.Total);

        public async Task RemoveFromCartAsync(int cartItemId)
        {
            // 1. Load all items from the JSON file
            var allItems = await _fileService.ReadAsync<CartItem>(CartFile);
            var userId = GetUserId();

            // 2. Find the specific item belonging to THIS user
            var itemToRemove = allItems.FirstOrDefault(i => i.Id == cartItemId && i.UserId == userId);

            if (itemToRemove != null)
            {
                // 3. Remove it from the list
                allItems.Remove(itemToRemove);

                // 4. Save the updated list back to the JSON file
                await _fileService.WriteAsync(allItems, CartFile);
            }
        }
    }
}