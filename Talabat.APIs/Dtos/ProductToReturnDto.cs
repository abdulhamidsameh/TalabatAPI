using Talabat.Core.Entities;

namespace Talabat.APIs.Dtos
{
	public class ProductToReturnDto
	{
        public int Id { get; set; }
        public string Name { get; set; } = null!;
		public string Description { get; set; } = null!;
		public string PictureUrl { get; set; } = null!;
		public decimal Price { get; set; }

		public int BrandId { get; set; } 
		virtual public string Brand { get; set; } = null!;

		public int CategoryId { get; set; }
		virtual public string Category { get; set; } = null!; 
	}
}
