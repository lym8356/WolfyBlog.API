using System;
using AutoMapper;
using WolfyBlog.API.DTOs;
using WolfyBlog.API.Entities;

namespace WolfyBlog.API.Profiles
{
	public class AlbumMappingProfile : Profile
    {
		public AlbumMappingProfile()
		{
			CreateMap<AlbumForCreationDTO, Album>();
		}
	}
}

