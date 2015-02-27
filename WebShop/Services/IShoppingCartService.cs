using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebShop.Models;

namespace WebShop.Services
{
	public interface IShoppingCartService
	{
		Task<Cart> GetCartAsync();
		Task<Cart> GetCartAsync(string id);
		Task SaveAsync(Cart cart);
		Task AddItemToCartAsync(Cart cart, string id);
		Task SubtractItemFromCartAsync(Cart cart, string id);
		Task RemoveItemFromCartAsync(Cart cart, string id);
		Task EmptyCartAsync(Cart cart);
		Task FillCartAsync(Cart cart, ICatalogService catalogService, IFinanceService financeService);
	}
}