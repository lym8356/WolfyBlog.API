using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WolfyBlog.API.DTOs;
using WolfyBlog.API.Services;
using WolfyBlog.API.Services.Interfaces;

namespace WolfyBlog.API.Controllers
{
    [Route("api/[controller]")] // api/SiteLog
    [ApiController]
    public class SiteLogController : ControllerBase
	{
        private ISiteLogRepository _siteLogRepository;

        public SiteLogController(ISiteLogRepository siteLogRepository)
		{
            _siteLogRepository = siteLogRepository;
		}

        [HttpGet]
        public async Task<IActionResult> GetSiteLogs()
        {
            var siteLogsFromRepo = await _siteLogRepository.GetSiteLogsAsync();
            if (siteLogsFromRepo == null || siteLogsFromRepo.Count() <= 0)
            {
                return NotFound("No site logs found.");
            }
            return Ok(siteLogsFromRepo);
        }


        [HttpGet("{siteLogId}", Name = "GetSiteLog")]
        public async Task<IActionResult> GetSiteLog(int siteLogId)
        {
            if (!(await _siteLogRepository.SiteLogExistAsync(siteLogId)))
            {
                return NotFound("site log does not exist.");
            }
            var siteLogFromRepo = await _siteLogRepository.GetSiteLogAsync(siteLogId);
            return Ok(siteLogFromRepo);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateSiteLog([FromBody] SiteLogForCreationDTO siteLogForCreationDTO)
        {
            var siteLogToReturn = _siteLogRepository.CreateSiteLog(siteLogForCreationDTO);
            var result = await _siteLogRepository.SaveAsync();
            if (result) return CreatedAtRoute("GetSiteLog", new { SiteLogId = siteLogToReturn.Id }, siteLogToReturn);
            return BadRequest(new ProblemDetails { Title = "Problem creating site log" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{siteLogId}")]
        public async Task<IActionResult> EditSiteLog(int siteLogId, SiteLogForUpdateDTO siteLogForUpdateDTO)
        {
            if (!(await _siteLogRepository.SiteLogExistAsync(siteLogId)))
            {
                return NotFound("Site log does not exist.");
            }

            var siteLogFromRepo = await _siteLogRepository.GetSiteLogAsync(siteLogId);
            // map
            var siteLogToReturn = _siteLogRepository.EditSiteLog(siteLogFromRepo, siteLogForUpdateDTO);

            var result = await _siteLogRepository.SaveAsync();
            if (result) return Ok(siteLogToReturn);
            return BadRequest(new ProblemDetails { Title = "Problem editing site log" });

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{siteLogId}")]
        public async Task<IActionResult> DeleteSiteLog(int siteLogId)
        {
            if (!(await _siteLogRepository.SiteLogExistAsync(siteLogId)))
            {
                return NotFound("Site log does not exist.");
            }

            var siteLogFromRepo = await _siteLogRepository.GetSiteLogAsync(siteLogId);
            _siteLogRepository.DeleteSiteLog(siteLogFromRepo);
            var result = await _siteLogRepository.SaveAsync();
            if (result) return Ok();
            return BadRequest(new ProblemDetails { Title = "Problem deleting site log" });
        }
    }
}

