using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Models;

namespace WebShop.Services
{
	public interface ICatalogService
	{
		Product Find(string id);
		IQueryable<Product> GetProducts();
	}
}
