using System;
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
                return NotFound("No categories found.");
            }
            return Ok(articlesFromRepo);
        }

        [HttpGet("{articleId}")]
        public async Task<IActionResult> GetArticle(Guid articleId)
        {
            if (!(await _articleRepository.ArticleExistsAsync(articleId)))
            {
                return NotFound("Article does not exist.");
            }

            var articleFromRepo = await _articleRepository.GetArticleAsync(articleId);
            return Ok(articleFromRepo);
        }

        [HttpPost]
        public async Task<IActionResult> CreateArticle([FromBody] ArticleForCreationDTO articleForCreationDTO)
        {
            await _articleRepository.CreateArticleAsync(articleForCreationDTO);
            var result = await _articleRepository.SaveAsync();
            if (result) return Ok();
            return BadRequest(new ProblemDetails { Title = "Problem creating article" });
        }

        [HttpPut("{articleId}")]
        public async Task<IActionResult> EditArticle(Guid articleId, ArticleForUpdateDTO articleForUpdateDTO)
        {
            var articleToEdit = await _articleRepository.GetArticleAsync(articleId);
            if (articleToEdit == null)
            {
                return NotFound($"Article with ID {articleId} does not exist.");
            }
            await _articleRepository.EditArticleAsync(articleToEdit, articleForUpdateDTO);
            var result = await _articleRepository.SaveAsync();
            if (result) return Ok();
            return BadRequest(new ProblemDetails { Title = "Problem updating article" });
        }

        [HttpDelete("{articleId}")]
        public async Task<IActionResult> DeleteArticle(Guid articleId)
        {
            if (!(await _articleRepository.ArticleExistsAsync(articleId)))
            {
                return NotFound("Article does not exist.");
            }

            var articleFromRepo = await _articleRepository.GetArticleAsync(articleId);
            _articleRepository.DeleteArticleAsync(articleFromRepo);
            var result = await _articleRepository.SaveAsync();
            if (result) return Ok();
            return BadRequest(new ProblemDetails { Title = "Problem deleting article" });
        }
    }
}

