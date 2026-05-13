using Microsoft.AspNetCore.Mvc.RazorPages;
using LDPhotography.Models;

namespace LDPhotography.Pages.Portfolio
{
    public class SpringModel : PageModel
    {
        public string Title { get; set; } = "Spring Collection";
        public string Description { get; set; } = "A study in verdant renewal and the softening of light. This collection captures the Pacific Northwest as it awakens, focusing on the interplay of mist, moss, and the return of vibrant life to the river gorges.";
        
        public PortfolioImage HeroImage { get; set; } = new();
        public List<PortfolioImage> GalleryImages { get; set; } = new();

        public void OnGet()
        {
            var allImages = new List<PortfolioImage>
            {
                new PortfolioImage { Url = "https://images-pw.pixieset.com/elementfield/J3AAbRK/MossyEvergreen-00686f19-1500.jpg", AltText = "Mossy Evergreen", Caption = "The First Buds of the Gorge" },
                new PortfolioImage { Url = "https://images-pw.pixieset.com/elementfield/J3AAbRK/ShallowCreek-9cfcc9df-1500.jpg", AltText = "Shallow Creek", Caption = "Flowing Resilience" },
                new PortfolioImage { Url = "https://images-pw.pixieset.com/elementfield/ovrrE7b/WizardsHatatSunset-c73fc28c-1500.jpg", AltText = "Wizards Hat", Caption = "Golden Hour Arrival" },
                new PortfolioImage { Url = "https://images-pw.pixieset.com/elementfield/yWOe31a/ViewofColumbiaRiverGorge-3c916350-1500.jpg", AltText = "Columbia River Gorge", Caption = "Morning Mist over the Columbia" }
            };

            // Ansel Adams layout: First image is the feature, others are in the gallery
            HeroImage = allImages.First();
            GalleryImages = allImages.Skip(1).ToList();
        }
    }
}