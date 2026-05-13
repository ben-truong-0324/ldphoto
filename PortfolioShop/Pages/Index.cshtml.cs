using Microsoft.AspNetCore.Mvc.RazorPages;
using LDPhotography.Models;
using System.Collections.Generic;

namespace LDPhotography.Pages
{
    public class IndexModel : PageModel
    {
        public List<LDPortfolio> Portfolios { get; set; } = new();
        public List<LandscapePrint> AvailablePrints { get; set; } = new();
        public List<PrintCollection> PrintCollections { get; set; } = new();

        public void OnGet()
        {
            Portfolios = new List<LDPortfolio>
            {
                new LDPortfolio { 
                    Id = 1, Title = "Spring", Collection = "seasonal", SubCategory = "spring", 
                    ImageUrl = "/images/Greenery Wonders revised.jpg" 
                },
                new LDPortfolio { 
                    Id = 2, Title = "Summer", Collection = "seasonal", SubCategory = "summer", 
                    ImageUrl = "/images/Bodega Bay revised.jpg" 
                },
                new LDPortfolio { 
                    Id = 3, Title = "Autumn", Collection = "seasonal", SubCategory = "autumn", 
                    ImageUrl = "/images/Lovely Autumn.jpg" 
                },
                new LDPortfolio { 
                    Id = 4, Title = "Winter", Collection = "seasonal", SubCategory = "winter", 
                    ImageUrl = "/images/Winter wonderland.jpg" 
                }
            };

            AvailablePrints = new List<LandscapePrint>
            {
                new LandscapePrint { Id = 1, Title = "Spring Awakening", Price = 125.00m },
                new LandscapePrint { Id = 2, Title = "Summer Warmth", Price = 150.00m },
                new LandscapePrint { Id = 3, Title = "Autumn Colors", Price = 175.00m }
            };

            PrintCollections = new List<PrintCollection>
            {
                new PrintCollection { 
                    Slug = "seasonal", Title = "The Seasonal Collection", StartingPrice = 125.00m,
                    CoverImageUrl = "/images/Greenery Wonders revised.jpg" 
                }
            };
        }
    }
}