using System;
using System.Collections.Generic;
using System.Linq;
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
		public ActionResult Index()
		{
			Cart cart = shoppingCartService.GetCart();

			shoppingCartService.FillCart(cart, catalogService, financeService);

			return View(new ShoppingCartIndexViewModel() { Cart = cart });
		}

		public ActionResult CartSummary()
		{
			Cart cart = shoppingCartService.GetCart();

			return PartialView(cart.Items.Sum(x => x.Quantity));
		}

		[HttpPost]
		public ActionResult AddItemToCart(string id)
		{
			Cart cart = shoppingCartService.GetCart();

			shoppingCartService.AddItemToCart(cart, id);

			return Json(true);
		}

		[HttpPost]
		public ActionResult SubtractItemFromCart(string id)
		{
			Cart cart = shoppingCartService.GetCart();

			shoppingCartService.SubtractItemFromCart(cart, id);

			return Json(true);
		}

		[HttpPost]
		public ActionResult RemoveItemFromCart(string id)
		{
			Cart cart = shoppingCartService.GetCart();

			shoppingCartService.RemoveItemFromCart(cart, id);

			return Json(true);
		}

		[HttpPost]
		public ActionResult EmptyCart()
		{
			Cart cart = shoppingCartService.GetCart();

			shoppingCartService.EmptyCart(cart);

			return Json(true);
		}

		//[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
		//private Cart GetCart()
		//{
		//	Cart cart = null;

		//	// try to get cart for the user
		//	if (User.Identity.IsAuthenticated)
		//	{
		//		cart = shoppingCartService.GetCartForUser(User.Identity.Name);
		//	}

		//	// try to get the cart from the cookie
		//	HttpCookie cookie = Request.Cookies["_cartId"];
		//	if (cookie != null && !String.IsNullOrEmpty(cookie.Value)) // cookie present
		//	{
		//		Cart cookieCart = shoppingCartService.GetCart(cookie.Value);

		//		if (cart == null && cookieCart != null && User.Identity.IsAuthenticated) // associate the cart with the user if authenticated
		//		{
		//			shoppingCartService.SetUser(cookieCart, User.Identity.Name);
		//			shoppingCartService.Save(cart);
		//			cart = cookieCart;
		//		}

		//		if (cart != null && cookieCart != null && cart.Id != cookieCart.Id) // the user is probably logging from another PC or browser
		//			cart = shoppingCartService.MergeCarts(cart, cookieCart); // we merge the two carts and give priority to the for the authenticated user
		//		else if (cookieCart != null)
		//			cart = cookieCart;
		//	}

		//	if (cart == null) cart = shoppingCartService.CreateNewCart(User.Identity.Name);

		//	// update the cookie
		//	cookie = new HttpCookie("_cartId");
		//	cookie.Value = cart.Id;
		//	cookie.Expires = DateTime.Now.AddMonths(1);
		//	Response.Cookies.Add(cookie);

		//	return cart;
		//}


	}
}