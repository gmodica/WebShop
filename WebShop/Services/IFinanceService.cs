using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Services
{
	public interface IFinanceService
	{
		double CalculateTax(double price);
		double Tax { get; }
	}
}
