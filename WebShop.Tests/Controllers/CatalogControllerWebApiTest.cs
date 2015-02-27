using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PagedList;
using WebShop.Controllers.WebApi;
using WebShop.Models;
using WebShop.Services;

namespace ShoppingWeb.Tests.Controllers
{
	[TestClass]
	public class CatalogControllerWebApiTest
	{
		protected Mock<ICatalogService> catalogService;

		protected int totalProducts = 21;

		[TestInitialize]
		public void SetupMocks()
		{

			List<Product> products = Enumerable.Range(1, totalProducts).Select(x => new Product() { Name = x.ToString() }).ToList();

			catalogService = new Mock<ICatalogService>();
			catalogService.Setup(m => m.GetProductsAsync()).Returns(Task.FromResult(products.AsQueryable<Product>()));
			catalogService.Setup(m => m.FindAsync("15")).Returns(Task.FromResult(products[14]));
		}

		[TestMethod]
		public async Task CatalogWebApiGet()
		{
			// Arrange
			CatalogController controller = new CatalogController(catalogService.Object);

			// Act
			IEnumerable<Product> result = await controller.Get() as IEnumerable<Product>;

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(totalProducts, result.Count());
		}

		[TestMethod]
		public async Task CatalogWebApiGetId()
		{
			// Arrange
			CatalogController controller = new CatalogController(catalogService.Object);

			// Act
			OkNegotiatedContentResult<Product> result = await controller.Get("15") as OkNegotiatedContentResult<Product>;

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual("15", result.Content.Name);
		}
	}
}
