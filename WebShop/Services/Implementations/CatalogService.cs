using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using WebShop.Models;

namespace WebShop.Services
{
	[XmlRoot("products")]
	public class CatalogService : ICatalogService
	{
		protected string file;
		protected List<Product> products;

		protected CatalogService()
		{

		}

		public static CatalogService Create(string file)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(CatalogService));

			CatalogService store = null;
			using(FileStream fileStream = new FileStream(file, FileMode.Open))
			{
				 store = (CatalogService)serializer.Deserialize(fileStream);
			}
			store.file = file;

			return store;
		}

		[XmlElement("product")]
		public List<Product> Products
		{
			get { return products; }
			set { products = value; }
		}

		public Task<Product> FindAsync(string id)
		{
			Product product = Products.Where(x => x.Id == id).FirstOrDefault();

			TaskCompletionSource<Product> tcs = new TaskCompletionSource<Product>();
			tcs.SetResult(product);

			return tcs.Task;
		}

		public Task<IQueryable<Product>> GetProductsAsync()
		{
			IQueryable<Product> result = products.AsQueryable();

			TaskCompletionSource<IQueryable<Product>> tcs = new TaskCompletionSource<IQueryable<Product>>();
			tcs.SetResult(result);

			return tcs.Task;
		}

		public Task<IQueryable<Product>> GetDealsAsync()
		{
			Random random = new Random(DateTime.Now.Millisecond);
			List<Product> deals = new List<Product>();
		
			for(int i = 0; i <= 3; i++)
				deals.Add(products[random.Next(products.Count)]);

			IQueryable<Product> result = deals.AsQueryable();

			TaskCompletionSource<IQueryable<Product>> tcs = new TaskCompletionSource<IQueryable<Product>>();
			tcs.SetResult(result);

			return tcs.Task;
		}
	}

}