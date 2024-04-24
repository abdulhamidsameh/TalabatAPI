using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.Dtos
{
	public class BasketItemDto
	{
		[Required]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; } = null!;

		[Required]
		public string PictureUrl { get; set; } = null!;

		[Required]
		[Range(0.1,double.MaxValue,ErrorMessage ="Price must be Greater than Zerro::")]
		public decimal Price { get; set; }

		[Required]
		public string Category { get; set; } = null!;

		[Required]
		public string Brand { get; set; } = null!;

		[Required]
		[Range(1,int.MaxValue,ErrorMessage ="Quantity Must be at Least one Item")]
		public int Quantity { get; set; }
	}
}