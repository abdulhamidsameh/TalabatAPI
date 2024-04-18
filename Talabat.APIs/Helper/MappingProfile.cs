using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;

namespace Talabat.APIs.Helper
{
	public class MappingProfile : Profile
	{
		private readonly IConfiguration _configuration;
		public MappingProfile(IConfiguration configuration)
		{
			_configuration = configuration;

			CreateMap<Product, ProductToReturnDto>()
				.ForMember(d => d.Brand, O => O.MapFrom(s => s.Brand.Name))
				.ForMember(d => d.Category, O => O.MapFrom(s => s.Category.Name))
				.ForMember(P => P.PictureUrl, O => O.MapFrom(S => $"{_configuration["ApiBaseUrl"]}/{S.PictureUrl}"));
		}
	}
}
