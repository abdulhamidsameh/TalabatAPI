using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.APIs.Errors;
using Talabat.APIs.Helper;
using Talabat.APIs.Middlewares;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Repostiory;
using Talabat.Repostiory.Data;

namespace Talabat.APIs
{
	// Onion Architecture Layers Naming
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var webApplicationBuilder = WebApplication.CreateBuilder(args);

			#region Configre Services
			// Add services to the container.

			webApplicationBuilder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			webApplicationBuilder.Services.AddEndpointsApiExplorer();
			webApplicationBuilder.Services.AddSwaggerGen();
			webApplicationBuilder.Services.AddDbContext<StoreContext>(options =>
			{
				options.UseLazyLoadingProxies().UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
			});

			webApplicationBuilder.Services.AddScoped(
				typeof(IGenricRepository<>), typeof(GenericRepository<>)
				);
			//webApplicationBuilder.Services.AddScoped<IGenricRepository<Product>, GenericRepository<Product>>();

			webApplicationBuilder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfile(webApplicationBuilder.Configuration)));

			webApplicationBuilder.Services.Configure<ApiBehaviorOptions>(options =>
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
			#endregion


			var app = webApplicationBuilder.Build();

			using var scope = app.Services.CreateScope();
			var services = scope.ServiceProvider;
			var _dbContext = services.GetRequiredService<StoreContext>();
			// Ask CLR for Creating Object From DbContext Explicitly

			var loggerFactory = services.GetRequiredService<ILoggerFactory>();

			try
			{
				await _dbContext.Database.MigrateAsync();
				await StoreContextSeed.SeedAsync(_dbContext);
			}
			catch (Exception ex)
			{
				var logger = loggerFactory.CreateLogger<Program>();
				logger.LogError(ex, "an error has been occured during apply the migration");
			}

			#region Configure Kestrel Middlewares

			app.UseMiddleware<ExceptionMiddleware>();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}


			app.UseHttpsRedirection();

			app.UseStaticFiles();


			app.MapControllers();

			#endregion



			app.Run();
		}
	}
}
