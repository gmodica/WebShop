using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

			FileStream fileStream = new FileStream(file, FileMode.Open);
			CatalogService store = (CatalogService)serializer.Deserialize(fileStream);
			store.file = file;

			return store;
		}

		[XmlElement("product")]
		public List<Product> Products
		{
			get { return products; }
			set { products = value; }
		}

		public Product Find(string id)
		{
			return Products.Where(x => x.Id == id).FirstOrDefault();
		}

		public IQueryable<Product> GetProducts()
		{
			return products.AsQueryable();
		}

		public IQueryable<Product> GetDeals()
		{
			Random random = new Random(DateTime.Now.Millisecond);
			List<Product> deals = new List<Product>();
		
			for(int i = 0; i <= 3; i++)
				deals.Add(products[random.Next(products.Count)]);

			return deals.AsQueryable();
		}
	}

}