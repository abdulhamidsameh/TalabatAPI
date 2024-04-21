using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications.Products_Specs
{
	public class ProductSpecParms
	{
		private const int MaxPageSize = 10;
		private int pageSize = 5;

		public int PageSize
		{
			get { return pageSize; }
			set
			{
				if (value > MaxPageSize || value < 0)
					pageSize = MaxPageSize;
				else
					pageSize = value;
			}
		}

		public int PageIndex { get; set; } = 1;

		public string? Sort { get; set; }
		public int? BrandId { get; set; }
		public int? CategoryId { get; set; }

        public string? Search { get; set; }
    }
}
