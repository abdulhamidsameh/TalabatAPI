
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Middlewares
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionMiddleware> _logger;
		private readonly IWebHostEnvironment _env;

		public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IWebHostEnvironment env)
		{
			_next = next;
			_logger = logger;
			_env = env;
		}


		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				// Take an Action With The Request

				await _next.Invoke(httpContext); // Go To The Next Middleware

				// Take an Action with The Response
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message); // Development Env
											  // Log Exception in (Databasem | Files) --> Production Env
				httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				httpContext.Response.ContentType = "application/json";

				var response = _env.IsDevelopment() ? new ApiExceptionResponse(500, ex.Message, ex.StackTrace.ToString())
					: new ApiExceptionResponse(500);

				var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

				var json = JsonSerializer.Serialize(response, options);

				await httpContext.Response.WriteAsync(json);
			}

		}
	}
}
