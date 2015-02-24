using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebShop.Models;

namespace WebShop.Services
{
	public class ShoppingCartService : IShoppingCartService
	{
		protected readonly ApplicationDbContext dbContext;

		protected ShoppingCartService(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public static ShoppingCartService Create(ApplicationDbContext dbContext)
		{
			ShoppingCartService service = new ShoppingCartService(dbContext);
			return service;
		}

		public Cart CreateNewCart(string userName)
		{
			Cart cart = new Cart();
			cart.User = dbContext.Users.Where(x => x.Email == userName).FirstOrDefault();
			dbContext.Carts.Add(cart);
			dbContext.SaveChanges();

			return cart;
		}

		public Cart MergeCarts(Cart primaryCart, Cart secondaryCart)
		{
			List<CartItem> items = new List<CartItem>();
			items.AddRange(secondaryCart.Items);

			secondaryCart.Items.Clear();

			foreach (CartItem item in items)
			{
				CartItem primaryItem = primaryCart.Items.Where(x => x.ProductId == item.ProductId).FirstOrDefault();
				if (primaryItem != null)
				{
					primaryItem.Quantity += item.Quantity;
					dbContext.CartItems.Remove(item);
				}
				else
					primaryCart.Items.Add(item);
			}

			dbContext.Carts.Remove(secondaryCart);
			dbContext.SaveChanges();

			return primaryCart;
		}


		public Cart GetCartForUser(string userName)
		{
			return dbContext.Carts.Include("Items").Where(x => x.User.UserName == userName).FirstOrDefault();
		}

		public Cart GetCart(string id)
		{
			return dbContext.Carts.Include("Items").Where(x => x.Id == id).FirstOrDefault();
		}

		public void Save()
		{
			dbContext.SaveChanges();
		}


		public void SetUser(Cart cart, string userName)
		{
			cart.User = dbContext.Users.Where(x => x.Email == userName).FirstOrDefault();
		}


		public void AddItemToCart(Cart cart, string id)
		{
			CartItem item = cart.Items.Where(x => x.ProductId == id).FirstOrDefault();
			if (item != null) item.Quantity++;
			else cart.Items.Add(new CartItem() { ProductId = id, Quantity = 1 });
			Save();
		}

		public void SubtractItemFromCart(Cart cart, string id)
		{
			CartItem item = cart.Items.Where(x => x.ProductId == id).FirstOrDefault();
			if (item != null && item.Quantity > 0)
			{
				item.Quantity--;
				//if (item.Quantity == 0) cart.Items.Remove(item);
				Save();
			}
		}

		public void RemoveItemFromCart(Cart cart, string id)
		{
			CartItem item = cart.Items.Where(x => x.ProductId == id).FirstOrDefault();
			if (item != null)
			{
				cart.Items.Remove(item);
				Save();
			}
		}

		public void EmptyCart(Cart cart)
		{
			cart.Items.Clear();
			Save();
		}


		public void FillCart(Cart cart, ICatalogService catalogService, IFinanceService financeService)
		{
			foreach (CartItem item in cart.Items)
			{
				item.Product = catalogService.Find(item.ProductId);
				item.Total = item.Quantity * item.Product.Price;
			}

			cart.SubTotal = cart.Items.Sum(x => x.Quantity * x.Product.Price);
			cart.Tax = financeService.Tax;
			cart.TaxTotal = financeService.CalculateTax(cart.SubTotal);
			cart.Total = cart.SubTotal + cart.TaxTotal;
		}
	}
}