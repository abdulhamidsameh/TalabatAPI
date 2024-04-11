using Microsoft.EntityFrameworkCore;
using Talabat.Repostiory.Data;

namespace Talabat.APIs
{
	public class Program
	{
		public static void Main(string[] args)
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
				options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
			});
			
			
			#endregion


			var app = webApplicationBuilder.Build();

			#region Configure Kestrel Middlewares
			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.MapControllers();

			#endregion

			app.Run();
		}
	}
}
