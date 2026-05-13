using LDPhotography.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Portfolio.Test.Tests
{
    [TestClass]
    public class IndexModelTests
    {
        [TestMethod]
        public void OnGet_PopulatesLDPortfoliosAndPrints()
        {
            // Arrange
            var pageModel = new IndexModel();

            // Act
            pageModel.OnGet();

            // Assert
            Assert.IsNotNull(pageModel.Portfolios, "LDPortfolios list should not be null.");
            Assert.HasCount(4, pageModel.Portfolios, "There should be exactly 4 seasons.");
            Assert.IsTrue(pageModel.Portfolios.Any(p => p.SeasonName == "Spring"), "The LDPortfolios should contain 'Spring'.");

            Assert.IsNotNull(pageModel.AvailablePrints, "AvailablePrints list should not be null.");
            Assert.HasCount(3, pageModel.AvailablePrints, "There should be exactly 3 landscape prints.");
            Assert.IsTrue(pageModel.AvailablePrints.Any(p => p.Title == "Oregon Trip"), "The prints should contain 'Oregon Trip'.");
        }
    }
}