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
		public ProductWithBrandAndCategorySpecifications(ProductSpecParms specParms)
			: base(
				  P =>
				  (string.IsNullOrEmpty(specParms.Search) || P.Name.Contains(specParms.Search)) &&
				  (!specParms.BrandId.HasValue || P.BrandId == specParms.BrandId.Value) &&
				  (!specParms.CategoryId.HasValue || P.CategoryId == specParms.CategoryId.Value)
				  )
		{
			AddIncludes();

			if (!string.IsNullOrEmpty(specParms.Sort))
				switch (specParms.Sort)
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

			ApplyPaginantion((specParms.PageIndex - 1) * specParms.PageSize, specParms.PageSize);
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
