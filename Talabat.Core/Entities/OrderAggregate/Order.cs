using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.OrderAggregate
{
	public class Order : BaseEntity
	{
        public Order()
        {
            
        }

		public Order(string bayerEmail, Address shippingAddress, DeliveryMethod? deliveryMethod, ICollection<OrderItem> items, decimal subTotal)
		{
			BayerEmail = bayerEmail;
			ShippingAddress = shippingAddress;
			DeliveryMethod = deliveryMethod;
			Items = items;
			SubTotal = subTotal;
		}

		public string BayerEmail { get; set; } = null!;
		public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
		public OrderStatus Status { get; set; } = OrderStatus.Pending;
		public Address ShippingAddress { get; set; } = null!;
        //public int DeliveryMethodId { get; set; } //FK
        public virtual DeliveryMethod? DeliveryMethod { get; set; } = null!; // Navigational Property
        public virtual ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
        public decimal SubTotal { get; set; }
		public decimal GetTotal()
			=> SubTotal + DeliveryMethod.Cost;
		public string PaymentIntentId { get; set; } = string.Empty;

    }
}
