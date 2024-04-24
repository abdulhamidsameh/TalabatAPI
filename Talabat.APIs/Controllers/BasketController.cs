﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;

namespace Talabat.APIs.Controllers
{
	public class BasketController : BaseApiController
	{
		private readonly IBasketRepository _basketRepository;
		private readonly IMapper _mapper;

		public BasketController(IBasketRepository basketRepository,IMapper mapper)
        {
			_basketRepository = basketRepository;
			_mapper = mapper;
		}

		[HttpGet] // GET : /api/basket/id
		public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
		{
			var basket = await _basketRepository.GetBasketAsync(id);
			return Ok(basket ?? new CustomerBasket(id));
		}

		[HttpPost] // POST : /api/basket
		public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
		{
			var mapping = _mapper.Map<CustomerBasketDto,CustomerBasket>(basket);
			var createdOrUpdatedBasket = await _basketRepository.UpdateBasketAsync(mapping);
			if (createdOrUpdatedBasket is null) return BadRequest(new ApiResponse(400));
			return Ok(createdOrUpdatedBasket);
		}

		[HttpDelete] // DELETE : /api/basket
		public async Task<ActionResult<bool>> DeleteBasket(string id)
		{
			return await _basketRepository.DeleteBasketAsync(id);
		}
	}
}
