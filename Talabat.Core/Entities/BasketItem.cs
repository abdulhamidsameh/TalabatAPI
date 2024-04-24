﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
	public class BasketItem
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public string PictureUrl { get; set; } = null!;
		public decimal Price { get; set; }
		public string Category { get; set; } = null!;
		public string Brand { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
