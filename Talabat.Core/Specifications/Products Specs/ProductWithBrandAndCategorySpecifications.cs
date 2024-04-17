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
        public ProductWithBrandAndCategorySpecifications()
			: base()
        {
            Includes.Add(P => P.Brand);
			Includes.Add(P => P.Category);
		}
        public ProductWithBrandAndCategorySpecifications(Expression<Func<Product,bool>> criteria)
            :this()
        {
            Criteria = criteria;
        }
    }
}
