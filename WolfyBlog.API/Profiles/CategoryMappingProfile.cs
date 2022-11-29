using AutoMapper;
using WolfyBlog.API.DTOs;
using WolfyBlog.API.Entities;

namespace WolfyBlog.API.Profiles
{
	public class CategoryMappingProfile : Profile
	{
		public CategoryMappingProfile()
		{
            CreateMap<Category, CategoryDTO>();
        }
	}
}

