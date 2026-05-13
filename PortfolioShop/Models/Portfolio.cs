namespace LDPhotography.Models
{
    public class LDPortfolio
    {
        public int Id { get; set; }
        public string Title { get; set; } // Replacing SeasonName for broader use
        public string Collection { get; set; }
        public string SubCategory { get; set; }
        public string SeasonName { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string LinkUrl { get; set; } = string.Empty;
    }
}