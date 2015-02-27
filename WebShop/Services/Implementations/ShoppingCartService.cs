using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
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

		public async Task<Cart> GetCartAsync()
		{
			Cart cart = null;

			// try to get cart for the user
			if (HttpContext.Current.User.Identity.IsAuthenticated)
			{
				cart = await GetCartForUserAsync(HttpContext.Current.User.Identity.Name);
			}

			// try to get the cart from the cookie
			HttpCookie cookie = HttpContext.Current.Request.Cookies["_cartId"];
			if (cookie != null && !String.IsNullOrEmpty(cookie.Value)) // cookie present
			{
				Cart cookieCart = await GetCartAsync(cookie.Value);

				if (cart == null && cookieCart != null && HttpContext.Current.User.Identity.IsAuthenticated) // associate the cart with the user if authenticated
				{
					SetUser(cookieCart, HttpContext.Current.User.Identity.Name);
					await SaveAsync(cookieCart);
					cart = cookieCart;
				}

				if (cart != null && cookieCart != null && cart.Id != cookieCart.Id) // the user is probably logging from another PC or browser
					cart = await MergeCartsAsync(cart, cookieCart); // we merge the two carts and give priority to the for the authenticated user
				else if (cookieCart != null)
					cart = cookieCart;
			}

			if (cart == null) cart = await CreateNewCartAsync(HttpContext.Current.User.Identity.Name);

			// update the cookie
			cookie = new HttpCookie("_cartId");
			cookie.Value = cart.Id;
			cookie.Expires = DateTime.Now.AddMonths(1);
			HttpContext.Current.Response.Cookies.Add(cookie);

			return cart;
		}

		public async Task<Cart> GetCartAsync(string id)
		{
			return await dbContext.Carts.Include("Items").Where(x => x.Id == id).FirstOrDefaultAsync();
		}

		public async Task SaveAsync(Cart cart)
		{
			cart.Date = DateTime.Now; // update the date so it reflects the last time this cart was used

			dbContext.Entry(cart).State = System.Data.Entity.EntityState.Modified;			
			await dbContext.SaveChangesAsync();
		}

		public async Task AddItemToCartAsync(Cart cart, string id)
		{
			CartItem item = cart.Items.Where(x => x.ProductId == id).FirstOrDefault();
			if (item != null) item.Quantity++;
			else cart.Items.Add(new CartItem() { ProductId = id, Quantity = 1 });
			await SaveAsync(cart);
		}

		public async Task SubtractItemFromCartAsync(Cart cart, string id)
		{
			CartItem item = cart.Items.Where(x => x.ProductId == id).FirstOrDefault();
			if (item != null && item.Quantity > 0)
			{
				item.Quantity--;
				//if (item.Quantity == 0) cart.Items.Remove(item);
				await SaveAsync(cart);
			}
		}

		public async Task RemoveItemFromCartAsync(Cart cart, string id)
		{
			CartItem item = cart.Items.Where(x => x.ProductId == id).FirstOrDefault();
			if (item != null)
			{
				cart.Items.Remove(item);
				await SaveAsync(cart);
			}
		}

		public async Task EmptyCartAsync(Cart cart)
		{
			cart.Items.Clear();
			await SaveAsync(cart);
		}


		public async Task FillCartAsync(Cart cart, ICatalogService catalogService, IFinanceService financeService)
		{
			foreach (CartItem item in cart.Items)
			{
				item.Product = await catalogService.FindAsync(item.ProductId);
				item.Total = item.Quantity * item.Product.Price;
			}

			cart.SubTotal = cart.Items.Sum(x => x.Quantity * x.Product.Price);
			cart.Tax = financeService.Tax;
			cart.TaxTotal = financeService.CalculateTax(cart.SubTotal);
			cart.Total = cart.SubTotal + cart.TaxTotal;
		}

		protected void SetUser(Cart cart, string userName)
		{
			cart.User = dbContext.Users.Where(x => x.Email == userName).FirstOrDefault();
		}


		protected async Task<Cart> CreateNewCartAsync(string userName)
		{
			Cart cart = new Cart();
			cart.User = dbContext.Users.Where(x => x.Email == userName).FirstOrDefault();
			dbContext.Carts.Add(cart);
			await dbContext.SaveChangesAsync();

			return cart;
		}

		protected async Task<Cart> MergeCartsAsync(Cart primaryCart, Cart secondaryCart)
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
			await dbContext.SaveChangesAsync();

			return primaryCart;
		}


		protected async Task<Cart> GetCartForUserAsync(string userName)
		{
			return await dbContext.Carts.Include("Items").Where(x => x.User.UserName == userName).FirstOrDefaultAsync();
		}
	}
}