using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Erp.Models
{
	public class ErpDbContext : DbContext
	{

		public ErpDbContext()
			: base("ErpConnection")
		{
		}

		public static ErpDbContext Create()
		{
			return new ErpDbContext();
		}

		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderItem> OrderItems { get; set; }
	}
}
