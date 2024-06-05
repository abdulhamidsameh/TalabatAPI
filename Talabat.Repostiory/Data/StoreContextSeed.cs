using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Repostiory.Data
{
	public static class StoreContextSeed
	{
		public async static Task SeedAsync(StoreContext _dbContext)
		{
			if (_dbContext.ProductBrands.Count() == 0)
			{
				var brandsData = File.ReadAllText("../Talabat.Repostiory/Data/DataSeed/brands.json");
				var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
				if (brands is not null && brands.Count > 0)
				{
					foreach (var brand in brands)
					{
						_dbContext.Set<ProductBrand>().Add(brand);
					}
					await _dbContext.SaveChangesAsync();
				}
			}

			if (_dbContext.ProductCategories.Count() == 0)
			{
				var categoriesData = File.ReadAllText("../Talabat.Repostiory/Data/DataSeed/categories.json");
				var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoriesData);
				if (categories is not null && categories.Count > 0)
				{
					foreach (var categorie in categories)
					{
						_dbContext.Set<ProductCategory>().Add(categorie);
					}
					await _dbContext.SaveChangesAsync();
				}
			}
			
			if (_dbContext.Products.Count() == 0)
			{
				var productsData = File.ReadAllText("../Talabat.Repostiory/Data/DataSeed/products.json");
				var products = JsonSerializer.Deserialize<List<Product>>(productsData);
				if (products is not null && products.Count > 0)
				{
					foreach (var product in products)
					{
						_dbContext.Set<Product>().Add(product);
					}
					await _dbContext.SaveChangesAsync();
				}
			}

			if (_dbContext.DeliveryMethods.Count() == 0)
			{
				var deliveryMethodData = File.ReadAllText("../Talabat.Repostiory/Data/DataSeed/delivery.json");
				var deliveryMethod = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodData);
				if (deliveryMethod is not null && deliveryMethod.Count > 0)
				{
					foreach (var delivery in deliveryMethod)
					{
						_dbContext.Set<DeliveryMethod>().Add(delivery);
					}
					await _dbContext.SaveChangesAsync();
				}
			}
		}
	}
}
