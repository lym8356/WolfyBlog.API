using System;
using AutoMapper;
using WolfyBlog.API.DTOs;
using WolfyBlog.API.Entities;

namespace WolfyBlog.API.Profiles
{
	public class TagMappingProfile : Profile
	{
		public TagMappingProfile()
		{
			CreateMap<Tag, TagDTO>();
			CreateMap<ArticleTag, TagDTO>()
				.ForMember(d => d.Id, o => o.MapFrom(at => at.Tag.Id))
				.ForMember(d => d.Title, o => o.MapFrom(at => at.Tag.Title));
		}
	}
}

