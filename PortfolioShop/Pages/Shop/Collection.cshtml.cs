using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LDPhotography.Models;
using System.Collections.Generic;

namespace LDPhotography.Pages.Shop
{
    public class CollectionModel : PageModel
    {
        public PrintCollection CurrentCollection { get; set; } = new();

        public IActionResult OnGet(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug)) return RedirectToPage("/Shop/Index");

            // Define the 3 collections based on the URLs you provided
            switch (slug.ToLower())
            {
                case "seasonaltravels":
                    CurrentCollection = new PrintCollection
                    {
                        Title = "Seasonal Travels",
                        Description = "A curated selection of landscapes captured across changing seasons.",
                        StartingPrice = 125.00m,
                        AvailablePrints = new List<PortfolioImage>
                        {
                            // Add the actual image URLs from your Pixieset gallery here
                            new PortfolioImage { Url = "https://images-pw.pixieset.com/elementfield/ovrrE7b/WizardsHatatSunset-c73fc28c-1500.jpg", Caption = "Wizard's Hat" },
                            new PortfolioImage { Url = "https://images-pw.pixieset.com/elementfield/yWOe31a/ViewofColumbiaRiverGorge-3c916350-1500.jpg", Caption = "Gorge Mist" }
                        }
                    };
                    break;

                case "autumnseries2024":
                    CurrentCollection = new PrintCollection
                    {
                        Title = "Autumn Series 2024",
                        Description = "Warm tones and falling leaves. Limited edition prints from the 2024 fall expedition.",
                        StartingPrice = 175.00m,
                        AvailablePrints = new List<PortfolioImage>
                        {
                            new PortfolioImage { Url = "https://images-pw.pixieset.com/elementfield/J3AAbRK/ShallowCreek-9cfcc9df-1500.jpg", Caption = "Shallow Creek" }
                        }
                    };
                    break;

                case "oregontrip2024":
                    CurrentCollection = new PrintCollection
                    {
                        Title = "Oregon Trip 2024",
                        Description = "The rugged beauty of the Pacific Northwest coast and deep inland forests.",
                        StartingPrice = 150.00m,
                        AvailablePrints = new List<PortfolioImage>
                        {
                            new PortfolioImage { Url = "https://images-pw.pixieset.com/elementfield/J3AAbRK/MossyEvergreen-00686f19-1500.jpg", Caption = "Mossy Evergreen" }
                        }
                    };
                    break;

                default:
                    return RedirectToPage("/Shop/Index");
            }

            return Page();
        }
    }
}