using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities.OrderAggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Service.Contract;

namespace Talabat.Service.OrderService
{
	public class OrderService : IOrderService
	{
		private readonly IBasketRepository _basketRepo;
		private readonly IUnitOfWork _unitOfWork;

		public OrderService(
			IBasketRepository basketRepo,
			IUnitOfWork unitOfWork
			)
        {
			_basketRepo = basketRepo;
			_unitOfWork = unitOfWork;
		}
        public Task<Order> CreateOrderAsync(string bayerEmail, string basketId, string deliveryMethodId, Address shippingAddress)
		{
			throw new NotImplementedException();
		}

		public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Order> GetOrderByIdForUserAsync(string bayerEmail, int orderId)
		{
			throw new NotImplementedException();
		}

		public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string bayerEmail)
		{
			throw new NotImplementedException();
		}
	}
}
