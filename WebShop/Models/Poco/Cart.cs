using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebShop.Models
{

	[Table("Cart")]
	public class Cart
	{
		public Cart()
		{
			this.Id = Guid.NewGuid().ToString();
			Date = DateTime.Now;
			Items = new List<CartItem>();
		}

		[MaxLength(40)]
		public string Id { get; set; }

		public ApplicationUser User { get; set; }

		public DateTime? Date { get; set; }

		public ICollection<CartItem> Items { get; set; }

		[NotMapped]
		[DisplayFormat(DataFormatString = "{0:c}")]
		public double SubTotal { get; set; }

		[NotMapped]
		[DisplayFormat(DataFormatString = "{0:c}")]
		public double Tax { get; set; }

		[NotMapped]
		[DisplayFormat(DataFormatString = "{0:c}")]
		public double TaxTotal { get; set; }

		[NotMapped]
		[DisplayFormat(DataFormatString = "{0:c}")]
		public double Total { get; set; }
	}
}