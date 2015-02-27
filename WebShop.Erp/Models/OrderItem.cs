using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Erp.Models
{
	public class OrderItem
	{
		public OrderItem()
		{
			this.Id = Guid.NewGuid().ToString();
		}

		[MaxLength(40)]
		public string Id { get; set; }

		[MaxLength(40)]
		public string ProductId { get; set; }

		public double Price { get; set; }

		public int Quantity { get; set; }

		public Order Order {get; set;}
	}
}
