using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LDPhotography.Models;
using System.Collections.Generic;
using System.Linq;

namespace LDPhotography.Pages.Portfolio
{
    public class SeasonModel : PageModel
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public PortfolioImage HeroImage { get; set; } = new();
        public List<PortfolioImage> GalleryImages { get; set; } = new();

        public IActionResult OnGet(string seasonName)
        {
            if (string.IsNullOrWhiteSpace(seasonName))
            {
                return RedirectToPage("/Index");
            }

            // Shared pool of known images (Replace the gallery items with your actual Pixieset URLs)
            var imgSpring = new PortfolioImage { Url = "https://images-pw.pixieset.com/elementfield/J3AAbRK/MossyEvergreen-00686f19-1500.jpg", AltText = "Mossy Evergreen", Caption = "The First Buds of the Gorge" };
            var imgSummer = new PortfolioImage { Url = "https://images-pw.pixieset.com/elementfield/ovrrE7b/WizardsHatatSunset-c73fc28c-1500.jpg", AltText = "Wizards Hat", Caption = "Golden Hour Arrival" };
            var imgAutumn = new PortfolioImage { Url = "https://images-pw.pixieset.com/elementfield/J3AAbRK/ShallowCreek-9cfcc9df-1500.jpg", AltText = "Shallow Creek", Caption = "Flowing Resilience" };
            var imgWinter = new PortfolioImage { Url = "https://images-pw.pixieset.com/elementfield/yWOe31a/ViewofColumbiaRiverGorge-3c916350-1500.jpg", AltText = "Columbia River Gorge", Caption = "Morning Mist over the Columbia" };

            // Dynamically load content based on the URL parameter
            switch (seasonName.ToLower())
            {
                case "spring":
                    Title = "Spring Collection";
                    Description = "A study in verdant renewal and the softening of light. This collection captures the Pacific Northwest as it awakens, focusing on the interplay of mist, moss, and the return of vibrant life.";
                    HeroImage = imgSpring;
                    GalleryImages = new List<PortfolioImage> { imgSummer, imgAutumn, imgWinter }; 
                    break;

                case "summer":
                    Title = "Summer Series";
                    Description = "Harsh shadows and brilliant sunsets. The summer collection highlights the stark contrasts and vivid warmth of the coast during the longest days of the year.";
                    HeroImage = imgSummer;
                    GalleryImages = new List<PortfolioImage> { imgSpring, imgAutumn, imgWinter };
                    break;

                case "autumn":
                    Title = "Autumn Series";
                    Description = "The transition of colors and cooling streams. Capturing the fleeting amber and gold hues reflecting off shallow creeks before the freeze.";
                    HeroImage = imgAutumn;
                    GalleryImages = new List<PortfolioImage> { imgSpring, imgSummer, imgWinter };
                    break;

                case "winter":
                    Title = "Winter Collection";
                    Description = "Desolate beauty and crystalline stillness. A focus on monochromatic landscapes, frost-covered gorges, and the quiet power of the frozen river.";
                    HeroImage = imgWinter;
                    GalleryImages = new List<PortfolioImage> { imgSpring, imgSummer, imgAutumn };
                    break;

                default:
                    // If a user types an invalid season in the URL, send them home
                    return RedirectToPage("/Index");
            }

            return Page();
        }
    }
}