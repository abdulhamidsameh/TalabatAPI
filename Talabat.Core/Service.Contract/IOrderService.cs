using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Core.Service.Contract
{
	public interface IOrderService
	{
		Task<Order> CreateOrderAsync(string bayerEmail, string basketId, int deliveryMethodId, Address shippingAddress);
		Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string bayerEmail);
		Task<Order> GetOrderByIdForUserAsync(string bayerEmail,int orderId);
		Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
	}
}
