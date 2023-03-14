using AutoMapper;
using WolfyBlog.API.DTOs;
using WolfyBlog.API.Entities;

namespace WolfyBlog.API.Profiles
{
	public class CommentMappingProfile : Profile
	{
		public CommentMappingProfile()
		{
            CreateMap<CommentForCreationDTO, Comment>();
        }
	}
}

