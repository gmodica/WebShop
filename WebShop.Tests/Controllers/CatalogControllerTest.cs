using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PagedList;
using WebShop.Controllers;
using WebShop.Models;
using WebShop.Services;

namespace ShoppingWeb.Tests.Controllers
{
	[TestClass]
	public class CatalogControllerTest
	{
		protected Mock<ICatalogService> catalogService;
		protected Mock<ISettingsService> settingsService;

		protected int totalProducts = 35;
		protected int pageSize = 10;

		[TestInitialize]
		public void SetupMocks()
		{

			List<Product> products = Enumerable.Range(1, totalProducts).Select(x => new Product() { Name = x.ToString() }).ToList();

			catalogService = new Mock<ICatalogService>();
			catalogService.Setup(m => m.GetProductsAsync()).Returns(Task.FromResult(products.AsQueryable<Product>()));
			catalogService.Setup(m => m.FindAsync("15")).Returns(Task.FromResult(products[14]));

			settingsService = new Mock<ISettingsService>();
			settingsService.Setup(m => m.PageSize).Returns(pageSize);
		}

		[TestMethod]
		public async Task CatalogIndex()
		{
			// Arrange
			CatalogController controller = new CatalogController(catalogService.Object, settingsService.Object);

			// Act
			ViewResult result = await controller.Index(null) as ViewResult;

			// Assert
			IPagedList<Product> pagedList = result.Model as IPagedList<Product>;
			Assert.IsNotNull(pagedList);
			Assert.AreEqual(pageSize, pagedList.Count);
			Assert.AreEqual(Math.Ceiling((float)totalProducts / (float)pageSize), pagedList.PageCount);
			Assert.AreEqual(1, pagedList.PageNumber);
			Assert.AreEqual(totalProducts, pagedList.TotalItemCount);
		}

		[TestMethod]
		public async Task CatalogIndexWithPage()
		{
			// Arrange
			CatalogController controller = new CatalogController(catalogService.Object, settingsService.Object);

			// Act
			ViewResult result = await controller.Index(2) as ViewResult;

			// Assert
			Assert.IsNotNull(result);
			IPagedList<Product> pagedList = result.Model as IPagedList<Product>;
			Assert.IsNotNull(pagedList);
			Assert.AreEqual(pageSize, pagedList.Count);
			Assert.AreEqual(Math.Ceiling((float)totalProducts / (float)pageSize), pagedList.PageCount);
			Assert.AreEqual(2, pagedList.PageNumber);
			Assert.AreEqual(totalProducts, pagedList.TotalItemCount);
		}

		[TestMethod]
		public async Task CatalogProduct()
		{
			// Arrange
			CatalogController controller = new CatalogController(catalogService.Object, settingsService.Object);

			// Act
			ViewResult result = await controller.Product("15") as ViewResult;

			// Assert
			Assert.IsNotNull(result);
			Product product = result.Model as Product;
			Assert.IsNotNull(product);
			Assert.AreEqual("15", product.Name);
			
		}
	}
}
