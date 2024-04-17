using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.Products_Specs;

namespace Talabat.APIs.Controllers
{
	public class ProductsController : BaseApiController
	{
		private readonly IGenricRepository<Product> _productRepo;

		public ProductsController(IGenricRepository<Product> productRepo)
		{
			_productRepo = productRepo;
		}

		[HttpGet]
		public async Task<IActionResult> GetProducts()
		{
			var spec = new ProductWithBrandAndCategorySpecifications();
			var products = await _productRepo.GetAllWithSpecAsync(spec);
			return Ok(products);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Product>> GetProduct(int id)
		{
			var product =  await _productRepo.GetAsync(id);

			if(product == null)
				return NotFound(new {Message = "Not Found",StatusCode = 404});

			return Ok(product);
		}
	}
}
