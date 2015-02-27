using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebShop.Models
{
	public class CheckoutIndexViewModel
	{
		public Cart Cart { get; set; }
	}

	public class CheckoutThanksViewModel
	{
		public string OrderId { get; set; }
	}
}