using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace WebShop.Services
{
	public class FinanceService : IFinanceService
	{
		public double CalculateTax(double price)
		{
			return Tax * price;
		}

		protected double? tax = null;

		public double Tax
		{
			get
			{
				if (!tax.HasValue)
				{
					double taxAux;
					if (double.TryParse(WebConfigurationManager.AppSettings["erp:tax"], out taxAux))
						tax = taxAux;
					else
						tax = 0;
				}
				return tax.Value;
			}
		}
	}
}