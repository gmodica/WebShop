using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebShop;
using WebShop.Controllers;
using WebShop.Models;
using WebShop.Services;

namespace WebShop.Tests.Controllers
{
	[TestClass]
	public class HomeControllerTest
	{
		protected Mock<ICatalogService> catalogService;

		[TestInitialize]
		public void SetupMocks()
		{

			List<Product> products = Enumerable.Range(1, 3).Select(x => new Product() { Name = x.ToString() }).ToList();

			catalogService = new Mock<ICatalogService>();
			catalogService.Setup(m => m.GetDealsAsync()).Returns(Task.FromResult(products.AsQueryable<Product>()));
		}

		[TestMethod]
		public async Task HomeIndex()
		{
			// Arrange
			HomeController controller = new HomeController(catalogService.Object);

			// Act
			ViewResult result = await controller.Index() as ViewResult;

			// Assert
			Assert.IsNotNull(result);
			HomeViewModel model = result.Model as HomeViewModel;
			Assert.IsNotNull(model);
			Assert.AreEqual(3, model.Deals.Count);
		}

		[TestMethod]
		public void HomeAbout()
		{
			// Arrange
			HomeController controller = new HomeController(catalogService.Object);

			// Act
			ViewResult result = controller.About() as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void HomeContact()
		{
			// Arrange
			HomeController controller = new HomeController(catalogService.Object);

			// Act
			ViewResult result = controller.Contact() as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}
	}
}
