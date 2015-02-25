using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WebShop.Services;
using WebShop.Models;

namespace WebShop.Controllers
{
	public class CatalogController : Controller
	{
		protected readonly ICatalogService catalogService;
		protected readonly ISettingsService settingsService;

		public CatalogController(ICatalogService productService, ISettingsService settingsService)
		{
			this.catalogService = productService;
			this.settingsService = settingsService;
		}


		public ActionResult Index(int? page)
		{
			var pageNumber = page ?? 1;
			var pagedResults = catalogService.GetProducts().ToPagedList(pageNumber, settingsService.PageSize);

			return View(pagedResults);
		}

		public ActionResult Product(string id)
		{
			Product product = catalogService.Find(id);

			if (product == null) return HttpNotFound();

			return View(product);
		}

		public ActionResult WebApi()
		{
			return View();
		}
	}
}