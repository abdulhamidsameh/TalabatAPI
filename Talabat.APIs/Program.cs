using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Net;
using System.Text.Json;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helper;
using Talabat.APIs.Middlewares;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Repostiory;
using Talabat.Repostiory.Data;

namespace Talabat.APIs
{
	// Onion Architecture Layers Naming

	//In-Memory Database and Redis
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var webApplicationBuilder = WebApplication.CreateBuilder(args);

			#region Configre Services

			// Add services to the container.

			webApplicationBuilder.Services.AddControllers();

			webApplicationBuilder.Services.AddSwaggerServices();

			webApplicationBuilder.Services.AddDbContext<StoreContext>(options =>
			{
				options.UseLazyLoadingProxies().UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
			});

			webApplicationBuilder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfile(webApplicationBuilder.Configuration)));

			webApplicationBuilder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
			{
				var connection = webApplicationBuilder.Configuration.GetConnectionString("Redis");
				return ConnectionMultiplexer.Connect(connection);
			});

			webApplicationBuilder.Services.AddApplicationServices();

			#endregion


			var app = webApplicationBuilder.Build();

			using var scope = app.Services.CreateScope();
			var services = scope.ServiceProvider;
			var _dbContext = services.GetRequiredService<StoreContext>();
			// Ask CLR for Creating Object From DbContext Explicitly

			var loggerFactory = services.GetRequiredService<ILoggerFactory>();
			var logger = loggerFactory.CreateLogger<Program>();

			try
			{
				await _dbContext.Database.MigrateAsync();
				await StoreContextSeed.SeedAsync(_dbContext);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "an error has been occured during apply the migration");
			}

			#region Configure Kestrel Middlewares

			///app.Use(async (httpContext, _next) =>
			///{
			///	try
			///	{
			///		// Take an Action With The Request
			///		await _next.Invoke(httpContext); // Go To The Next Middleware
			///		// Take an Action with The Response
			///	}
			///	catch (Exception ex)
			///	{
			///		logger.LogError(ex.Message); // Development Env
			///									 // Log Exception in (Databasem | Files) --> Production Env
			///		httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			///		httpContext.Response.ContentType = "application/json";
			///		var response = app.Environment.IsDevelopment() ? new ApiExceptionResponse(500, ex.Message, ex.StackTrace.ToString())
			///			: new ApiExceptionResponse(500);
			///		var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
			///		var json = JsonSerializer.Serialize(response, options);
			///		await httpContext.Response.WriteAsync(json);
			///	}
			///});

			app.UseMiddleware<ExceptionMiddleware>();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwaggerMiddleware();
			}

			//app.UseStatusCodePagesWithRedirects("/errors/{0}");
			app.UseStatusCodePagesWithReExecute("/errors/{0}");

			app.UseHttpsRedirection();

			app.UseStaticFiles();


			app.MapControllers();

			#endregion



			app.Run();
		}
	}
}
