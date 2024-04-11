﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;

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
			var products = await _productRepo.GetAllAsync();
			return Ok(products);
		}
	}
}
