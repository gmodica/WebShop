using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebShop.Models;
using WebShop.Services;

namespace WebShop.Controllers
{
	[Authorize]
	public class CheckoutController : Controller
	{
		protected readonly IShoppingCartService shoppingCartService;
		protected readonly ICatalogService catalogService;
		protected readonly IFinanceService financeService;

		public CheckoutController(IShoppingCartService shoppingCartService, ICatalogService catalogService, IFinanceService financeService)
		{
			this.shoppingCartService = shoppingCartService;
			this.catalogService = catalogService;
			this.financeService = financeService;
		}

		public ActionResult Index()
		{
			Cart cart = shoppingCartService.GetCartForUser(User.Identity.Name);

			shoppingCartService.FillCart(cart, catalogService, financeService);

			return View(cart);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Checkout()
		{
			Cart cart = shoppingCartService.GetCartForUser(User.Identity.Name);

			shoppingCartService.EmptyCart(cart);

			return RedirectToAction("Thanks");
		}

		public ActionResult Thanks()
		{
			return View();
		}
	}
}