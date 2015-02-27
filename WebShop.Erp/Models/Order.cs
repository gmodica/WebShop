using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Erp.Models
{
	public class Order
	{
		public Order()
		{
			this.Id = Guid.NewGuid().ToString();
			Items = new List<OrderItem>();
		}

		[MaxLength(40)]
		public string Id { get; set; }

		public DateTime? Date { get; set; }

		[MaxLength(50)]
		public string FirstName { get; set; }

		[MaxLength(50)]
		public string LastName { get; set; }

		[MaxLength(10)]
		public string Title { get; set; }

		[MaxLength(50)]
		public string Address { get; set; }

		[MaxLength(50)]
		public string House { get; set; }

		[MaxLength(10)]
		public string Zip { get; set; }

		[MaxLength(50)]
		public string City { get; set; }

		[MaxLength(255)]
		public string Email { get; set; }

		public double Tax { get; set; }

		public ICollection<OrderItem> Items { get; set; }
	}
}
