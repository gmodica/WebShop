using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using WebShop.Models;
using WebShop.Services;

namespace WebShop.Controllers
{
	public class ShoppingCartController : Controller
	{
		protected readonly IShoppingCartService shoppingCartService;
		protected readonly ICatalogService catalogService;
		protected readonly IFinanceService financeService;

		public ShoppingCartController(IShoppingCartService shoppingCartService, ICatalogService catalogService, IFinanceService financeService)
		{
			this.shoppingCartService = shoppingCartService;
			this.catalogService = catalogService;
			this.financeService = financeService;
		}

		[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
		public async Task<ActionResult> Index()
		{
			Cart cart = await shoppingCartService.GetCartAsync();

			await shoppingCartService.FillCartAsync(cart, catalogService, financeService);

			return View(new ShoppingCartIndexViewModel() { Cart = cart });
		}

		public async Task<ActionResult> CartSummary()
		{
			Cart cart = await shoppingCartService.GetCartAsync();

			return View(cart.Items.Sum(x => x.Quantity));
		}

		[HttpPost]
		public async Task<ActionResult> AddItemToCart(string id)
		{
			Cart cart = await shoppingCartService.GetCartAsync();

			await shoppingCartService.AddItemToCartAsync(cart, id);

			return Json(true);
		}

		[HttpPost]
		public async Task<ActionResult> SubtractItemFromCart(string id)
		{
			Cart cart = await shoppingCartService.GetCartAsync();

			await shoppingCartService.SubtractItemFromCartAsync(cart, id);

			return Json(true);
		}

		[HttpPost]
		public async Task<ActionResult> RemoveItemFromCart(string id)
		{
			Cart cart = await shoppingCartService.GetCartAsync();

			await shoppingCartService.RemoveItemFromCartAsync(cart, id);

			return Json(true);
		}

		[HttpPost]
		public async Task<ActionResult> EmptyCart()
		{
			Cart cart = await shoppingCartService.GetCartAsync();

			await shoppingCartService.EmptyCartAsync(cart);

			return Json(true);
		}		

	}
}