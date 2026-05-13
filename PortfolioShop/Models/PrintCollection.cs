using System.Collections.Generic;

namespace LDPhotography.Models
{
    public class PrintCollection
    {
        public string Slug { get; set; } = string.Empty; // seasonaltravels, oregontrip2024, etc.
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CoverImageUrl { get; set; } = string.Empty;
        public decimal StartingPrice { get; set; }
        public List<PortfolioImage> AvailablePrints { get; set; } = new();
    }
}