﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.Products_Specs;

namespace Talabat.APIs.Controllers
{
	public class ProductsController : BaseApiController
	{
		private readonly IGenricRepository<Product> _productRepo;
		private readonly IGenricRepository<ProductCategory> _categoryRepo;
		private readonly IGenricRepository<ProductBrand> _brandRepo;
		private readonly IMapper _mapper;

		public ProductsController(IGenricRepository<Product> productRepo,
			IGenricRepository<ProductCategory> categoryRepo,
			IGenricRepository<ProductBrand> brandRepo,
			IMapper mapper)
		{
			_productRepo = productRepo;
			_categoryRepo = categoryRepo;
			_brandRepo = brandRepo;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts(string? sort, int? brandId, int? categoryId)
		{
			var spec = new ProductWithBrandAndCategorySpecifications(sort,brandId,categoryId);
			var products = await _productRepo.GetAllWithSpecAsync(spec);
			return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
		}

		[ProducesResponseType(typeof(ProductToReturnDto),StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
		[HttpGet("{id}")]
		public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
		{
			var spec = new ProductWithBrandAndCategorySpecifications(id);
			var product = await _productRepo.GetwithSpecAsync(spec);

			if (product == null)
				return NotFound(new ApiResponse(404));

			return Ok(_mapper.Map<Product, ProductToReturnDto>(product));
		}

		[HttpGet("brands")] // GET : /api/products/brands
		public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
		{
			var brands = await _brandRepo.GetAllAsync();
			return Ok(brands);
		}

		[HttpGet("categories")] // GET : /api/products/categories
		public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
		{
			var categories = await _categoryRepo.GetAllAsync();
			return Ok(categories);
		}
	}
}
