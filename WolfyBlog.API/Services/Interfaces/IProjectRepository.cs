using System;
using WolfyBlog.API.DTOs;
using WolfyBlog.API.Entities;

namespace WolfyBlog.API.Services.Interfaces
{
	public interface IProjectRepository
	{
		Task<bool> ProjectExistAsync(int id);
		Task<IEnumerable<Project>> GetProjectsAsync();
		Task<Project> GetProjectAsync(int id);
		Project CreateProject(ProjectForCreationDTO projectForCreationDTO);
		Project EditProject(Project projectFromRepo, ProjectForUpdateDTO projectForUpdateDTO);
		void DeleteProject(Project project);
        Task<bool> SaveAsync();
    }
}

