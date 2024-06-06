﻿using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.APIs.Dtos
{
	public class OrderDto
	{
        [Required]
        public string BuyerEmail { get; set; } = null!;
        [Required]
        public string BasketId { get; set; } = null!;
        [Required]
        public int DeliveryMethodId { get; set; }
        public AddressDto ShippingAddress { get; set; } = null!;
    }
}
