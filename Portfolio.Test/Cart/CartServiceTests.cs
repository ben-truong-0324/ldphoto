//using LDPhotography.Pages.Portfolio;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Linq;

//namespace Portfolio.Test.CartTests
//{

//    [TestClass]
//    public class CartServiceTests
//    {
//        [TestMethod]
//        public async Task AddToCart_WritesToInternalList()
//        {
//            // Arrange
//            // Use a specific test file so we don't overwrite production data
//            var fileService = new JsonFileDataService("data/test_cart.json");
//            var cart = new CartService(fileService, new MockHttpContextAccessor());

//            // Act
//            await cart.AddToCartAsync("print_01", "Oregon Coast", 150.00m, 1);
//            var items = await cart.GetCartItemsAsync();

//            // Assert
//            Assert.AreEqual(1, items.Count);
//            Assert.AreEqual("Oregon Coast", items[0].ProductName);
            
//            // Cleanup
//            if (File.Exists("data/test_cart.json")) File.Delete("data/test_cart.json");
//        }
//    }
//}