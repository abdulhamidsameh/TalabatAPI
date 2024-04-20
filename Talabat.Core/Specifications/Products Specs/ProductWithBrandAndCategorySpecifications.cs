using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.Products_Specs
{
	public class ProductWithBrandAndCategorySpecifications : BaseSpecifications<Product>
	{
		public ProductWithBrandAndCategorySpecifications(string? sort)
			: base()
		{
			AddIncludes();

			if (!string.IsNullOrEmpty(sort))
				switch (sort)
				{
					case "PriceAsc":
						OrderBy = P => P.Price;
						break;
					case "PriceDesc":
						OrderByDesc = P => P.Price;
						break;
					case "NameDesc":
						OrderByDesc = P => P.Name; 
						break;
					default:
						OrderBy = P => P.Name;
						break;
				}
			else
				OrderBy = P => P.Name;

		}
		public ProductWithBrandAndCategorySpecifications(int id)
			: base(P => P.Id == id)
		{
			AddIncludes();
		}
		private void AddIncludes()
		{
			Includes.Add(P => P.Brand);
			Includes.Add(P => P.Category);
		}
	}
}
