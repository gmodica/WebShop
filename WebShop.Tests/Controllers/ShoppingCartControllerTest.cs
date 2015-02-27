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
	public class ShoppingCartControllerTest
	{
		protected Mock<ICatalogService> catalogService;
		protected Mock<IShoppingCartService> shoppingCartService;
		protected Mock<IFinanceService> financeService;

		protected int totalProducts = 35;
		protected int pageSize = 10;

		[TestInitialize]
		public void SetupMocks()
		{

			List<Product> products = Enumerable.Range(1, totalProducts).Select(x => new Product() { Id = x.ToString(), Name = x.ToString(), Price = x }).ToList();

			Cart cart = new Cart()
			{
				Items = new List<CartItem>() {
					new CartItem() { ProductId = "1", Quantity = 1},
					new CartItem() { ProductId = "2", Quantity = 2},
				}
			};

			catalogService = new Mock<ICatalogService>();
			catalogService.Setup(m => m.GetProductsAsync()).Returns(Task.FromResult(products.AsQueryable<Product>()));
			catalogService.Setup(m => m.FindAsync("15")).Returns(Task.FromResult(products[14]));

			shoppingCartService = new Mock<IShoppingCartService>();
			shoppingCartService.Setup(m => m.GetCartAsync()).Returns(Task.FromResult(cart));

			financeService = new Mock<IFinanceService>();
			financeService.Setup(m => m.Tax).Returns(.21);
			financeService.Setup(m => m.CalculateTax(0)).Returns(0);
		}

		[TestMethod]
		public async Task ShoppingCartIndex()
		{
			// Arrange
			ShoppingCartController controller = new ShoppingCartController(shoppingCartService.Object, catalogService.Object, financeService.Object);

			// Act
			ViewResult result = await controller.Index() as ViewResult;

			// Assert
			Assert.IsNotNull(result);
			ShoppingCartIndexViewModel model = result.Model as ShoppingCartIndexViewModel;
			List<CartItem> items = model.Cart.Items.ToList();
			Assert.AreEqual(2, items.Count);
			Assert.AreEqual("1", items[0].ProductId);
			Assert.AreEqual("2", items[1].ProductId);
		}

		
	}
}
