using System;
using AutoMapper;
using WolfyBlog.API.DTOs;
using WolfyBlog.API.Entities;

namespace WolfyBlog.API.Profiles
{
	public class SiteLogMappingProfile : Profile
	{
		public SiteLogMappingProfile()
		{
            CreateMap<SiteLogForCreationDTO, SiteLog>();
        }
	}
}

