using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LDPhotography.Models;
using System.Collections.Generic;

namespace LDPhotography.Pages.Portfolio
{
    public class GalleryModel : PageModel
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public PortfolioImage HeroImage { get; set; } = new();
        public List<PortfolioImage> GalleryImages { get; set; } = new();

        public IActionResult OnGet(string collection, string subCategory)
        {
            if (string.IsNullOrWhiteSpace(collection)) return RedirectToPage("/Index");

            if (collection.ToLower() == "seasonal")
            {
                switch (subCategory?.ToLower())
                {
                    case "spring":
                        Title = "Spring Collection";
                        Description = "A study in verdant renewal and the softening of light.";
                        GalleryImages = new List<PortfolioImage> {
                            new PortfolioImage { Url = "/images/Chill vibe.jpg", Caption = "Chill Vibe", ProductUrl = "/Shop/Product/1" },
                            new PortfolioImage { Url = "/images/Drought creek.jpg", Caption = "Drought Creek", ProductUrl = "/Shop/Product/2" },
                            new PortfolioImage { Url = "/images/Greenery Wonders revised.jpg", Caption = "Greenery Wonders", ProductUrl = "/Shop/Product/3" },
                            new PortfolioImage { Url = "/images/Rocky Valley revised.jpg", Caption = "Rocky Valley", ProductUrl = "/Shop/Product/4" },
                            new PortfolioImage { Url = "/images/Universal switch.jpg", Caption = "Universal Switch", ProductUrl = "/Shop/Product/5" }
                        };
                        break;

                    case "summer":
                        Title = "Summer Series";
                        Description = "Harsh shadows and brilliant sunsets.";
                        GalleryImages = new List<PortfolioImage> {
                            new PortfolioImage { Url = "/images/Bodega Bay revised.jpg", Caption = "Bodega Bay", ProductUrl = "/Shop/Product/6" },
                            new PortfolioImage { Url = "/images/Crater Lake with colors.jpg", Caption = "Crater Lake", ProductUrl = "/Shop/Product/7" },
                            new PortfolioImage { Url = "/images/Pena Adobe.jpg", Caption = "Pena Adobe", ProductUrl = "/Shop/Product/8" },
                            new PortfolioImage { Url = "/images/The Secret Beach revised.jpg", Caption = "The Secret Beach", ProductUrl = "/Shop/Product/9" },
                            new PortfolioImage { Url = "/images/View of sea breeze.jpg", Caption = "Sea Breeze", ProductUrl = "/Shop/Product/10" }
                        };
                        break;

                    case "autumn":
                        Title = "Autumn Series";
                        Description = "The transition of colors and cooling streams.";
                        GalleryImages = new List<PortfolioImage> {
                            new PortfolioImage { Url = "/images/Connection to the trees.jpg", Caption = "Connection to the Trees", ProductUrl = "/Shop/Product/11" },
                            new PortfolioImage { Url = "/images/Lovely Autumn.jpg", Caption = "Lovely Autumn", ProductUrl = "/Shop/Product/12" },
                            new PortfolioImage { Url = "/images/Reflection magnicique.jpg", Caption = "Magnifique Reflection", ProductUrl = "/Shop/Product/13" }
                        };
                        break;

                    case "winter":
                        Title = "Winter Collection";
                        Description = "Desolate beauty and crystalline stillness.";
                        GalleryImages = new List<PortfolioImage> {
                            new PortfolioImage { Url = "/images/Winter wonderland.jpg", Caption = "Winter Wonderland", ProductUrl = "/Shop/Product/14" }
                        };
                        break;

                    default:
                        return RedirectToPage("/Index");
                }
            }
            else 
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }
    }
}