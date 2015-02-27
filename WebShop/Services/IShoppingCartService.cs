using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebShop.Models;

namespace WebShop.Services
{
	public interface IShoppingCartService
	{
		Cart GetCart();
		Cart GetCart(string id);
		void Save(Cart cart);
		void AddItemToCart(Cart cart, string id);
		void SubtractItemFromCart(Cart cart, string id);
		void RemoveItemFromCart(Cart cart, string id);
		void EmptyCart(Cart cart);
		void FillCart(Cart cart, ICatalogService catalogService, IFinanceService financeService);

		//Cart GetCartForUser(string userName);
		//Cart CreateNewCart(string userName);
		//Cart MergeCarts(Cart primaryCart, Cart secondaryCart);
		//void SetUser(Cart cart, string userName);

	}
}