using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WebShop.Services;
using WebShop.Models;
using System.Threading.Tasks;

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


		public async Task<ActionResult> Index(int? page)
		{
			var pageNumber = page ?? 1;
			var pagedResults = (await catalogService.GetProductsAsync()).ToPagedList(pageNumber, settingsService.PageSize);

			return View(pagedResults);
		}

		public async Task<ActionResult> Product(string id)
		{
			Product product = await catalogService.FindAsync(id);

			if (product == null) return HttpNotFound();

			return View(product);
		}

		public ActionResult WebApi()
		{
			return View();
		}
	}
}