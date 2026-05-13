using LDPhotography.Pages.Portfolio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Portfolio.Test.PortfolioTests
{
    [TestClass]
    public class PortfolioTests
    {
        [TestMethod]
        public void SpringModel_OnGet_CorrectlyPartitionsImages()
        {
            // Arrange
            var pageModel = new SpringModel();

            // Act
            pageModel.OnGet();

            // Assert
            Assert.IsNotNull(pageModel.HeroImage.Url, "Hero image must be assigned.");
            Assert.IsTrue(pageModel.GalleryImages.Count > 0, "Gallery should contain the remaining images.");
            
            // Ensure the Hero is not duplicated in the gallery (Ansel Adams style)
            bool isDuplicate = pageModel.GalleryImages.Any(x => x.Url == pageModel.HeroImage.Url);
            Assert.IsFalse(isDuplicate, "Hero image should not appear in the gallery carousel.");
        }
    }
}