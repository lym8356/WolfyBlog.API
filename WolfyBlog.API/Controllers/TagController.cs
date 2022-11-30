using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WolfyBlog.API.Entities;
using WolfyBlog.API.Services;

namespace WolfyBlog.API.Controllers
{
    [Route("api/[controller]")] // api/Tag
    [ApiController]
    public class TagController : ControllerBase
    {
        private ITagRepository _tagRepository;
        public TagController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetTags()
        {
            var tagsFromRepo = await _tagRepository.GetTagsAsync();
            if (tagsFromRepo == null || tagsFromRepo.Count() <= 0)
            {
                return NotFound("No tags found.");
            }
            return Ok(tagsFromRepo);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateTag(Tag tag)
        {
            _tagRepository.CreateTagAsync(tag);
            var result = await _tagRepository.SaveAsync();
            if (result) return Ok(tag);
            return BadRequest(new ProblemDetails { Title = "Problem creating tag" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{tagId}")]
        public async Task<IActionResult> EditTag(int tagId, Tag tag)
        {
            if (!(await _tagRepository.TagExistsAsync(tagId)))
            {
                return NotFound("Tag does not exist.");
            }

            var tagFromRepo = await _tagRepository.GetTagAsync(tagId);

            tagFromRepo.Title = tag.Title;
            var result = await _tagRepository.SaveAsync();
            if (result) return Ok();
            return BadRequest(new ProblemDetails { Title = "Problem editing tag" });

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{tagId}")]
        public async Task<IActionResult> DeleteTag(int tagId)
        {
            if (!(await _tagRepository.TagExistsAsync(tagId)))
            {
                return NotFound("Category does not exist.");
            }

            var tagFromRepo = await _tagRepository.GetTagAsync(tagId);
            _tagRepository.DeleteTagAsync(tagFromRepo);
            var result = await _tagRepository.SaveAsync();
            if (result) return Ok();
            return BadRequest(new ProblemDetails { Title = "Problem deleting tag" });
        }
    }
}

