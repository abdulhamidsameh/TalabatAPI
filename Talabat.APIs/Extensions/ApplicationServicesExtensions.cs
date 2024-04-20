﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.APIs.Helper;
using Talabat.Core.Repositories.Contract;
using Talabat.Repostiory;

namespace Talabat.APIs.Extensions
{
	public static class ApplicationServicesExtensions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddScoped(
				typeof(IGenricRepository<>), typeof(GenericRepository<>)
			);
			//webApplicationBuilder.Services.AddScoped<IGenricRepository<Product>, GenericRepository<Product>>();


			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = (actionContext) =>
				{
					var errors = actionContext.ModelState.Where(P => P.Value?.Errors.Count > 0)
														  .SelectMany(P => P.Value.Errors)
														  .Select(E => E.ErrorMessage)
														  .ToList();
					var response = new ApiValidationErrorResponse()
					{
						Errors = errors
					};
					return new BadRequestObjectResult(response);
				};
			});
			return services;
		}
	}
}
