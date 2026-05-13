using System.Collections.Generic;

namespace Portfolio.Core.Models
{
    public class Product
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        // Base price is a starting point; final price depends on the selected format/framing
        public decimal BasePrice { get; set; } 
        public string ImageUrl { get; set; } = string.Empty;

        // Configuration for what this specific photo can be sold as
        public List<ProductFormat> AvailableFormats { get; set; } = new();
        public List<PrintFinish> AvailableFinishes { get; set; } = new();
        public bool OffersFraming { get; set; }
        public Dictionary<string, decimal> Pricing { get; set; } = new();
    }
}