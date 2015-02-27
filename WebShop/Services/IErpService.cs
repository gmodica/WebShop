using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Erp.Models;

namespace WebShop.Services
{
	public interface IErpService
	{
		IQueryable<Order> Orders { get; }
		void Save(Order order);
	}
}
