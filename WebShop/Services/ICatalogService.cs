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
		Task<Product> FindAsync(string id);
		Task<IQueryable<Product>> GetProductsAsync();
		Task<IQueryable<Product>> GetDealsAsync();
	}
}
