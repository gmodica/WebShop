using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebShop.Erp.Models;

namespace WebShop.Services
{
	public class ErpService : IErpService
	{
		private ErpDbContext context;

		public ErpService()
		{
			context = ErpDbContext.Create();
		}

		public IQueryable<Order> Orders
		{
			get
			{
				return context.Orders;
			}
		}


		public void Save(Order order)
		{
			if (context.Orders.Find(order.Id) == null)
			{
				context.Orders.Add(order);
			}
			context.SaveChanges();
		}
	}
}