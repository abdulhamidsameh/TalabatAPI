using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Net;
using System.Text;
using System.Text.Json;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helper;
using Talabat.APIs.Middlewares;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.identity;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Service.Contract;
using Talabat.Repostiory;
using Talabat.Repostiory._Identity;
using Talabat.Repostiory.Data;
using Talabat.Service.AuthSrervice;
using Talabat.Service.OrderService;
namespace Talabat.APIs
{
	// Onion Architecture Layers Naming

	// In-Memory Database and Redis

	// Connect to Redis Server using Redily

	public class Program
	{
		public static async Task Main(string[] args)
		{
			var webApplicationBuilder = WebApplication.CreateBuilder(args);

			#region Configre Services

			// Add services to the container.

			webApplicationBuilder.Services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationIdentityDbContext>();

			webApplicationBuilder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters()
					{
						ValidateIssuer = true,
						ValidIssuer = webApplicationBuilder.Configuration["JWT:ValidIssuer"],
						ValidateAudience = true,
						ValidAudience = webApplicationBuilder.Configuration["JWT:ValidAudence"],
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(webApplicationBuilder.Configuration["JWT:AuthKey"] ?? string.Empty)),
						ValidateLifetime = true,
						ClockSkew = TimeSpan.Zero
					};
				});

			webApplicationBuilder.Services.AddControllers()
				.AddNewtonsoftJson(options =>
				{
					options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
				});

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

			webApplicationBuilder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
			{
				options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("IdentityConnection"));
			});

			webApplicationBuilder.Services.AddScoped(typeof(IAuthService), typeof(AuthService));

			webApplicationBuilder.Services.AddApplicationServices();

			webApplicationBuilder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

			webApplicationBuilder.Services.AddScoped(typeof(IOrderService), typeof(OrderService));

			#endregion


			var app = webApplicationBuilder.Build();

			using var scope = app.Services.CreateScope();
			var services = scope.ServiceProvider;
			var _dbContext = services.GetRequiredService<StoreContext>();
			var _IdentitydbContext = services.GetRequiredService<ApplicationIdentityDbContext>();
			// Ask CLR for Creating Object From DbContext Explicitly

			var loggerFactory = services.GetRequiredService<ILoggerFactory>();
			var logger = loggerFactory.CreateLogger<Program>();

			try
			{
				await _dbContext.Database.MigrateAsync();
				await _IdentitydbContext.Database.MigrateAsync();
				await StoreContextSeed.SeedAsync(_dbContext);
				var _userManger = services.GetRequiredService<UserManager<ApplicationUser>>();
				await ApplicationIdentityDataSeed.SeedUserAsync(_userManger);
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

			app.UseAuthentication();
			app.UseAuthorization();


			app.MapControllers();

			#endregion



			app.Run();
		}
	}
}
