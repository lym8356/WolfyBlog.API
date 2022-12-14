using System;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WolfyBlog.API.DTOs;
using WolfyBlog.API.Services;

namespace WolfyBlog.API.Controllers
{
    [Route("api/[controller]")] // api/Article
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private IArticleRepository _articleRepository;
        public ArticleController(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetArticles()
        {
            var articlesFromRepo = await _articleRepository.GetArticlesAsync();
            if (articlesFromRepo == null || articlesFromRepo.Count() <= 0)
            {
                return NotFound("No articles found.");
            }
            return Ok(articlesFromRepo);
        }

        [HttpGet("{articleId}", Name = "GetArticle")]
        public async Task<IActionResult> GetArticle(Guid articleId)
        {
            if (!(await _articleRepository.ArticleExistsAsync(articleId)))
            {
                return NotFound("Article does not exist.");
            }

            var articleFromRepo = await _articleRepository.GetArticleAsync(articleId);
            return Ok(articleFromRepo);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateArticle([FromBody] ArticleForCreationDTO articleForCreationDTO)
        {
            var articleToReturn = await _articleRepository.CreateArticleAsync(articleForCreationDTO);
            var result = await _articleRepository.SaveAsync();
            //if (result) return Ok();
            if (result) return CreatedAtRoute("GetArticle", new { ArticleId = articleToReturn.Id }, articleToReturn);
            return BadRequest(new ProblemDetails { Title = "Problem creating article" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{articleId}")]
        public async Task<IActionResult> EditArticle(Guid articleId, ArticleForUpdateDTO articleForUpdateDTO)
        {
            var articleToEdit = await _articleRepository.FindArticleByIdAsync(articleId);
            if (articleToEdit == null)
            {
                return NotFound($"Article with ID {articleId} does not exist.");
            }
            var temp = await _articleRepository.EditArticleAsync(articleToEdit, articleForUpdateDTO);
            var result = await _articleRepository.SaveAsync();
            if (result)
            {
                // cannot send back temp because it creates empty tags ? need to be fixed
                var articleToReturn = await _articleRepository.GetArticleAsync(temp.Id);
                return Ok(articleToReturn);
            }
            return BadRequest(new ProblemDetails { Title = "Problem updating article" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{articleId}")]
        public async Task<IActionResult> DeleteArticle(Guid articleId)
        {
            if (!(await _articleRepository.ArticleExistsAsync(articleId)))
            {
                return NotFound("Article does not exist.");
            }

            var articleFromRepo = await _articleRepository.FindArticleByIdAsync(articleId);
            _articleRepository.DeleteArticle(articleFromRepo);
            var result = await _articleRepository.SaveAsync();
            if (result) return Ok();
            return BadRequest(new ProblemDetails { Title = "Problem deleting article" });
        }
    }
}

