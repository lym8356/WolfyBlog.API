using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WolfyBlog.API.DTOs;
using WolfyBlog.API.Entities;
using WolfyBlog.API.Services.Interfaces;

namespace WolfyBlog.API.Controllers
{
    [Route("api/[controller]")] // api/Project
    [ApiController]
    public class ProjectController : ControllerBase
	{
		private IProjectRepository _projectRepository;
        public ProjectController(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var projectsFromRepo = await _projectRepository.GetProjectsAsync();
            if (projectsFromRepo == null || projectsFromRepo.Count() <= 0)
            {
                return NotFound("No projects found.");
            }
            return Ok(projectsFromRepo);
        }

        [HttpGet("{projectId}", Name = "GetProject")]
        public async Task<IActionResult> GetProject(int projectId)
        {
            if (!(await _projectRepository.ProjectExistAsync(projectId)))
            {
                return NotFound("Project does not exist.");
            }
            var projectFromRepo = await _projectRepository.GetProjectAsync(projectId);
            return Ok(projectFromRepo);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] ProjectForCreationDTO projectForCreationDTO)
        {
            var projectToReturn = _projectRepository.CreateProject(projectForCreationDTO);
            var result = await _projectRepository.SaveAsync();
            if (result) return CreatedAtRoute("GetProject", new { ProjectId = projectToReturn.Id }, projectToReturn);
            return BadRequest(new ProblemDetails { Title = "Problem creating project" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{projectId}")]
        public async Task<IActionResult> EditProject(int projectId, ProjectForUpdateDTO projectForUpdateDTO)
        {
            if (!(await _projectRepository.ProjectExistAsync(projectId)))
            {
                return NotFound("Project does not exist.");
            }

            var projectFromRepo = await _projectRepository.GetProjectAsync(projectId);
            // map
            var projectToReturn = _projectRepository.EditProject(projectFromRepo, projectForUpdateDTO);

            var result = await _projectRepository.SaveAsync();
            if (result) return Ok(projectToReturn);
            return BadRequest(new ProblemDetails { Title = "Problem editing project" });

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{projectId}")]
        public async Task<IActionResult> DeleteProject(int projectId)
        {
            if (!(await _projectRepository.ProjectExistAsync(projectId)))
            {
                return NotFound("Project does not exist.");
            }

            var projectFromRepo = await _projectRepository.GetProjectAsync(projectId);
            _projectRepository.DeleteProject(projectFromRepo);
            var result = await _projectRepository.SaveAsync();
            if (result) return Ok();
            return BadRequest(new ProblemDetails { Title = "Problem deleting project" });
        }
    }
}

