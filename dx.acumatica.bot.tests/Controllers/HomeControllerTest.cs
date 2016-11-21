using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using dx.acumatica.bot.api;
using dx.acumatica.bot.api.Controllers;

namespace dx.acumatica.bot.tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Home Page", result.ViewBag.Title);
        }
    }
}
