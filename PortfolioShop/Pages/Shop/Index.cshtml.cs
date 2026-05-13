using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace PortfolioShop.Pages.Shop
{
    public class ShopProductViewModel
    {
        public string Id { get; set; } 
        public string Name { get; set; }
        public string Collection { get; set; }
        public string SubCategory { get; set; }
        public string ImageUrl { get; set; }
        public decimal BasePrice { get; set; }
    }

    public class IndexModel : PageModel
    {
        public Dictionary<string, Dictionary<string, List<ShopProductViewModel>>> GroupedProducts { get; set; } = new();

        public void OnGet()
        {
            var allProducts = new List<ShopProductViewModel>
            {
                // Spring
                new ShopProductViewModel { Id = "1", Name = "Chill Vibe", Collection = "Seasonal", SubCategory = "Spring", ImageUrl = "/images/Chill vibe.jpg", BasePrice = 125.00m },
                new ShopProductViewModel { Id = "2", Name = "Drought Creek", Collection = "Seasonal", SubCategory = "Spring", ImageUrl = "/images/Drought creek.jpg", BasePrice = 125.00m },
                new ShopProductViewModel { Id = "3", Name = "Greenery Wonders", Collection = "Seasonal", SubCategory = "Spring", ImageUrl = "/images/Greenery Wonders revised.jpg", BasePrice = 125.00m },
                new ShopProductViewModel { Id = "4", Name = "Rocky Valley", Collection = "Seasonal", SubCategory = "Spring", ImageUrl = "/images/Rocky Valley revised.jpg", BasePrice = 125.00m },
                new ShopProductViewModel { Id = "5", Name = "Universal Switch", Collection = "Seasonal", SubCategory = "Spring", ImageUrl = "/images/Universal switch.jpg", BasePrice = 125.00m },

                // Summer
                new ShopProductViewModel { Id = "6", Name = "Bodega Bay", Collection = "Seasonal", SubCategory = "Summer", ImageUrl = "/images/Bodega Bay revised.jpg", BasePrice = 125.00m },
                new ShopProductViewModel { Id = "7", Name = "Crater Lake", Collection = "Seasonal", SubCategory = "Summer", ImageUrl = "/images/Crater Lake with colors.jpg", BasePrice = 125.00m },
                new ShopProductViewModel { Id = "8", Name = "Pena Adobe", Collection = "Seasonal", SubCategory = "Summer", ImageUrl = "/images/Pena Adobe.jpg", BasePrice = 125.00m },
                new ShopProductViewModel { Id = "9", Name = "The Secret Beach", Collection = "Seasonal", SubCategory = "Summer", ImageUrl = "/images/The Secret Beach revised.jpg", BasePrice = 125.00m },
                new ShopProductViewModel { Id = "10", Name = "Sea Breeze", Collection = "Seasonal", SubCategory = "Summer", ImageUrl = "/images/View of sea breeze.jpg", BasePrice = 125.00m },

                // Autumn
                new ShopProductViewModel { Id = "11", Name = "Connection to the Trees", Collection = "Seasonal", SubCategory = "Autumn", ImageUrl = "/images/Connection to the trees.jpg", BasePrice = 125.00m },
                new ShopProductViewModel { Id = "12", Name = "Lovely Autumn", Collection = "Seasonal", SubCategory = "Autumn", ImageUrl = "/images/Lovely Autumn.jpg", BasePrice = 125.00m },
                new ShopProductViewModel { Id = "13", Name = "Reflection Magnifique", Collection = "Seasonal", SubCategory = "Autumn", ImageUrl = "/images/Reflection magnicique.jpg", BasePrice = 125.00m },

                // Winter
                new ShopProductViewModel { Id = "14", Name = "Winter Wonderland", Collection = "Seasonal", SubCategory = "Winter", ImageUrl = "/images/Winter wonderland.jpg", BasePrice = 125.00m }
            };

            GroupedProducts = allProducts
                .GroupBy(p => p.Collection)
                .ToDictionary(
                    collectionGroup => collectionGroup.Key,
                    collectionGroup => collectionGroup
                        .GroupBy(p => p.SubCategory)
                        .ToDictionary(
                            subGroup => subGroup.Key,
                            subGroup => subGroup.ToList()
                        )
                );
        }
    }
}