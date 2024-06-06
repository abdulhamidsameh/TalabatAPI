using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
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
        public async Task<Order> CreateOrderAsync(string bayerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
		{
			var basket = await _basketRepo.GetBasketAsync(basketId);
			var orderItems = new List<OrderItem>();
			if(basket?.Items?.Count > 0)
			{
				foreach(var item in basket.Items)
				{
					var product = await _unitOfWork.Repository<Product>().GetAsync(item.Id);
					var productItemOrderd = new ProductItemOrderd(product.Id, product.Name, product.PictureUrl); 
					var orderItem = new OrderItem(productItemOrderd,product.Price,item.Quantity);
					orderItems.Add(orderItem);
				}
			}

			var subTotal = orderItems.Sum(item => item.Price * item.Quantity);

			var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(deliveryMethodId);

			var order = new Order(
				bayerEmail: bayerEmail,
				shippingAddress: shippingAddress,
				deliveryMethod: deliveryMethod,
				items: orderItems,
				subTotal: subTotal
				);

			_unitOfWork.Repository<Order>().Add(order);
			return order;

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
