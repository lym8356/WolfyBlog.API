using System;
using AutoMapper;
using WolfyBlog.API.DTOs;
using WolfyBlog.API.Entities;

namespace WolfyBlog.API.Profiles
{
	public class ProjectMappingProfile : Profile
	{
		public ProjectMappingProfile()
		{
			CreateMap<ProjectForCreationDTO, Project>();
		}
	}
}

