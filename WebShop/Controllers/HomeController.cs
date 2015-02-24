using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebShop.Models;
using WebShop.Services;

namespace WebShop.Controllers
{
	public class HomeController : Controller
	{
		protected readonly ICatalogService catalogService;

		public HomeController(ICatalogService catalogService)
		{
			this.catalogService = catalogService;
		}

		public ActionResult Index()
		{
			var deals = catalogService.GetDeals();
			HomeViewModel model = new HomeViewModel() { Deals = deals.Take(3).ToList() };

			return View(model);
		}

		public ActionResult About()
		{
			return View();
		}

		public ActionResult Contact()
		{
			return View();
		}
	}
}