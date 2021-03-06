﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebShop.Erp.Models;
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
		protected readonly IErpService erpService;
		protected readonly ApplicationUserManager userManager;

		public CheckoutController(IShoppingCartService shoppingCartService, ICatalogService catalogService, IFinanceService financeService, IErpService erpService, ApplicationUserManager userManager)
		{
			this.shoppingCartService = shoppingCartService;
			this.catalogService = catalogService;
			this.financeService = financeService;
			this.erpService = erpService;
			this.userManager = userManager;
		}

		public async Task<ActionResult> Index()
		{
			Cart cart = await shoppingCartService.GetCartAsync();

			await shoppingCartService.FillCartAsync(cart, catalogService, financeService);

			return View(new CheckoutIndexViewModel() { Cart = cart });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Checkout()
		{
			Cart cart = await shoppingCartService.GetCartAsync();

			ApplicationUser user = userManager.FindById(User.Identity.GetUserId());

			Order order = new Order()
			{
				FirstName = user.FirstName,
				LastName = user.LastName,
				Title = user.Title.Value.ToString(),
				Address = user.Address,
				House = user.House,
				Zip = user.Zip,
				City = user.City,
				Email = user.Email,
				Tax = financeService.Tax,
				Date = DateTime.Now
			};
			foreach(CartItem cartItem in cart.Items)
			{
				Product product = await catalogService.FindAsync(cartItem.ProductId);
				order.Items.Add(new OrderItem()
				{
					ProductId = product.Id,
					Price = product.Price,
					Quantity = cartItem.Quantity
				});
			}
			await erpService.SaveAsync(order);

			await shoppingCartService.EmptyCartAsync(cart);

			return RedirectToAction("Thanks", new { order.Id });
		}

		public ActionResult Thanks(string id)
		{
			return View(new CheckoutThanksViewModel() { OrderId = id });
		}
	}
}