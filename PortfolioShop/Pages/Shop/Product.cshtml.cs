using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Portfolio.Core.Models;
using Portfolio.Core.Services;

namespace PortfolioShop.Pages.Shop
{
    public class ProductModel : PageModel
    {
        private readonly JsonFileDataService _fileService;
        private readonly CartService _cartService;
        private readonly IWebHostEnvironment _env;

        public ProductModel(JsonFileDataService fileService, CartService cartService, IWebHostEnvironment env)
        {
            _fileService = fileService;
            _cartService = cartService;
            _env = env;
        }

        public Product Product { get; set; } = new();

        [BindProperty]
        public ProductFormat SelectedFormat { get; set; }
        
        [BindProperty]
        public string SelectedSize { get; set; } = "4x6";

        [BindProperty]
        public PrintFinish SelectedFinish { get; set; }
        
        [BindProperty]
        public bool IsFramed { get; set; }

        [BindProperty]
        public int Quantity { get; set; } = 1;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var path = Path.Combine(_env.WebRootPath, "data", "products.json");
            var products = await _fileService.ReadAsync<Product>(path);
            
            Product = products.FirstOrDefault(p => p.Id == id);

            if (Product == null) 
            {
                return NotFound(); 
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(string productId)
        {
            var path = Path.Combine(_env.WebRootPath, "data", "products.json");
            var products = await _fileService.ReadAsync<Product>(path);
            var product = products.FirstOrDefault(p => p.Id == productId);

            if (product == null) return NotFound();

            decimal finalPrice = CalculateVariantPrice(product, SelectedFormat, SelectedSize, SelectedFinish, IsFramed);

            var cartItem = new CartItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ImageUrl = product.ImageUrl,
                Price = finalPrice,
                Quantity = this.Quantity,
                UserId = "session-user", 
                SelectedFormat = SelectedFormat,
                SelectedSize = SelectedSize, 
                SelectedFinish = SelectedFinish,
                IsFramed = IsFramed
            };

            await _cartService.AddToCartAsync(cartItem);

            return RedirectToPage("/Shop/Cart");
        }

        private decimal CalculateVariantPrice(Product product, ProductFormat format, string size, PrintFinish finish, bool isFramed)
        {
            string priceKey = format.ToString();

            if ((format == ProductFormat.StandardPrint || format == ProductFormat.Puzzle) && !string.IsNullOrEmpty(size))
            {
                priceKey = $"{format}_{size}";
            }
            
            decimal price = product.Pricing.ContainsKey(priceKey) 
                            ? product.Pricing[priceKey] 
                            : product.BasePrice;

            // UPDATE: Print Finish upcharge ONLY applies to Standard Prints now
            if (format == ProductFormat.StandardPrint)
            {
                switch (finish)
                {
                    case PrintFinish.Matte: price += 2.30m; break;
                    case PrintFinish.Glossy: price += 2.30m; break;
                    case PrintFinish.Lustre: price += 3.00m; break;
                    case PrintFinish.Metallic: price += 4.30m; break;
                    default: break; 
                }
            }

            // Framing applies to both Standard Prints and Premium Wall Art
            if (isFramed && (format == ProductFormat.StandardPrint || format == ProductFormat.WallArt))
            {
                price += 40.00m;
            }

            return price;
        }
    }
}