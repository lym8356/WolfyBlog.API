using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WolfyBlog.API.DTOs;
using WolfyBlog.API.Entities;
using WolfyBlog.API.Services;
using WolfyBlog.API.Services.Interfaces;

namespace WolfyBlog.API.Controllers
{
    [Route("api/[controller]")] // api/AboutPage
    [ApiController]
    public class AboutPageController : ControllerBase
    {
        private IAboutPageRepository _aboutPageRepository;
        public AboutPageController(IAboutPageRepository aboutPageRepository)
		{
            _aboutPageRepository = aboutPageRepository;
		}

        [HttpGet]
        public async Task<IActionResult> GetAboutPages()
        {
            var aboutPagesFromRepo = await _aboutPageRepository.GetAboutPagesAsync();
            if (aboutPagesFromRepo == null || aboutPagesFromRepo.Count() <= 0)
            {
                return NotFound("No about pages found.");
            }
            return Ok(aboutPagesFromRepo);
        }

        [HttpGet("getnotification")]
        public async Task<IActionResult> GetNotification()
        {
            if (!(await _aboutPageRepository.NotificationExistAsync()))
            {
                return NotFound("Notification does not exist.");
            }
            var notificationFromRepo = await _aboutPageRepository.GetNotificationAsync();
            return Ok(notificationFromRepo);
        }

        [HttpGet("{aboutPageId}", Name = "GetAboutPage")]

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateAboutPage([FromBody] AboutPage aboutPage)
        {
            var count = await _aboutPageRepository.CheckDuplicate(aboutPage.IsAboutSite, aboutPage.IsNotification);
            
            // check for duplicate about page
            if (count.Count() > 0)
            {
                return BadRequest("About page with the same category already exists");
            }
            
            var aboutPageToReturn = _aboutPageRepository.CreateAboutPage(aboutPage);
            var result = await _aboutPageRepository.SaveAsync();
            if (result) return CreatedAtRoute("GetAboutPage", new { AboutPageId = aboutPageToReturn.Id }, aboutPageToReturn);
            return BadRequest(new ProblemDetails { Title = "Problem creating about page" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{aboutPageId}")]
        public async Task<IActionResult> EditAboutPage(int aboutPageId, AboutPage newAboutPage)
        {
            if (!(await _aboutPageRepository.AboutPageExistAsync(aboutPageId)))
            {
                return NotFound("About page does not exist.");
            }

            var aboutPageFromRepo = await _aboutPageRepository.GetAboutPageAsync(aboutPageId);

            //_aboutPageRepository.EditAboutPage(aboutPageFromRepo, newAboutPage);
            aboutPageFromRepo.Content = newAboutPage.Content;
            var result = await _aboutPageRepository.SaveAsync();
            if (result) return Ok(aboutPageFromRepo);
            return BadRequest(new ProblemDetails { Title = "Problem editing about page" });

        }
    }
}

