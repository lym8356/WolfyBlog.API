using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WolfyBlog.API.Database;
using WolfyBlog.API.DTOs;
using WolfyBlog.API.Entities;
using WolfyBlog.API.Services.Interfaces;

namespace WolfyBlog.API.Services
{
	public class ProjectRepository : IProjectRepository
	{

        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ProjectRepository(DataContext context, IMapper mapper)
		{
            _mapper = mapper;
            _context = context;
        }

        public Project CreateProject(ProjectForCreationDTO projectForCreationDTO)
        {
            var project = _mapper.Map<Project>(projectForCreationDTO);
            _context.Projects.Add(project);
            return project;
        }

        public void DeleteProject(Project project)
        {
            _context.Projects.Remove(project);
        }

        public Project EditProject(Project projectFromRepo, ProjectForUpdateDTO projectForUpdateDTO)
        {
            _mapper.Map(projectForUpdateDTO, projectFromRepo);
            _context.Update(projectFromRepo);
            return projectFromRepo;
        }

        public async Task<Project> GetProjectAsync(int id)
        {
            return await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Project>> GetProjectsAsync()
        {
            return await _context.Projects.ToListAsync();
        }

        public async Task<bool> ProjectExistAsync(int id)
        {
            return await _context.Projects.AnyAsync(p => p.Id == id);
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}

