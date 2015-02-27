using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebShop.Models;
using WebShop.Services;

namespace WebShop.Controllers.WebApi
{
	public class CatalogController : ApiController
	{
		protected readonly ICatalogService catalogService;

		public CatalogController(ICatalogService catalogService)
		{
			this.catalogService = catalogService;
		}

		// GET: api/Catalog
		[HttpGet]
		public async Task<IEnumerable<Product>> Get()
		{
			return (await catalogService.GetProductsAsync()).ToList();
		}

		// GET: api/Catalog/5
		[HttpGet]
		[ResponseType(typeof(Product))]
		public async Task<IHttpActionResult> Get(string id)
		{
			Product product = await catalogService.FindAsync(id);
			if (product == null)
			{
				return NotFound();
			}

			return Ok(product);
		}
	}
}
