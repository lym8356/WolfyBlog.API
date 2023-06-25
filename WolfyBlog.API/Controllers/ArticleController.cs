using System.Dynamic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using WolfyBlog.API.DTOs;
using WolfyBlog.API.Helper;
using WolfyBlog.API.ResourceParameters;
using WolfyBlog.API.Services;

namespace WolfyBlog.API.Controllers
{
    [Route("api/[controller]")] // api/Article
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private IArticleRepository _articleRepository;
        private readonly IUrlHelper _urlHelper;

        public ArticleController(IArticleRepository articleRepository,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor)
        {
            _articleRepository = articleRepository;

            if (actionContextAccessor?.ActionContext is null)
            {
                throw new ArgumentNullException(nameof(actionContextAccessor));
            }

            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        private string GenerateResourceURL(
            PaginationResourceParameters paginationParams,
            ResourceUriType type,
            string routeName,
            Dictionary<string, object> searchParams)
        {
            var queryParams = new ExpandoObject() as IDictionary<string, Object>;
            queryParams.Add("pageNumber", paginationParams.PageNumber);
            queryParams.Add("pageSize", paginationParams.PageSize);

            //var url = type switch
            //{
            //    ResourceUriType.PreviousPage => _urlHelper.Link(routeName,
            //        new
            //        {
            //            //keyword = searchParams.Keyword,
            //            searchParams,
            //            pageNumber = paginationParams.PageNumber - 1,
            //            pageSize = paginationParams.PageSize
            //        }),
            //    ResourceUriType.NextPage => _urlHelper.Link(routeName,
            //        new
            //        {
            //            //keyword = searchParams.Keyword,
            //            searchParams,
            //            pageNumber = paginationParams.PageNumber + 1,
            //            pageSize = paginationParams.PageSize
            //        }),
            //    _ => _urlHelper.Link(routeName,
            //        new
            //        {
            //            //keyword = searchParams.Keyword,
            //            searchParams,
            //            pageNumber = paginationParams.PageNumber,
            //            pageSize = paginationParams.PageSize
            //        })
            //};

            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    queryParams["pageNumber"] = paginationParams.PageNumber - 1;
                    break;
                case ResourceUriType.NextPage:
                    queryParams["pageNumber"] = paginationParams.PageNumber + 1;
                    break;
            }

            foreach(var param in searchParams)
            {
                queryParams.Add(param.Key, param.Value);
            }

            return _urlHelper.Link(routeName, queryParams);

            //if (url == null)
            //{
            //    throw new Exception("Failed to generate URL, incorrect parameters");
            //}

            //return url;
        }

        [HttpGet(Name = "GetArticles")]
        [HttpHead]
        public async Task<IActionResult> GetArticles(
            [FromQuery] ArticleResourceParameters searchParams,
            [FromQuery] PaginationResourceParameters paginationParams
            )
        {
            var articlesFromRepo = await _articleRepository.GetArticlesAsync(
                searchParams.Keyword,
                paginationParams.PageSize,
                paginationParams.PageNumber);

            if (articlesFromRepo == null || !articlesFromRepo.Any())
            {
                return NotFound("No articles found.");
            }

            var previousPageLink = articlesFromRepo.HasPrevious
                ? GenerateResourceURL(
                    paginationParams, ResourceUriType.PreviousPage, "GetArticles",
                    new Dictionary<string, object> { { "keyword", searchParams.Keyword} })
                : null;

            var nextPageLink = articlesFromRepo.HasNext
                ? GenerateResourceURL(
                    paginationParams, ResourceUriType.NextPage, "GetArticles",
                    new Dictionary<string, object> { { "keyword", searchParams.Keyword } })
                : null;

            var paginationMetadata = new
            {
                previousPageLink,
                nextPageLink,
                totalCount = articlesFromRepo.TotalCount,
                pageSize = articlesFromRepo.PageSize,
                currentPage = articlesFromRepo.CurrentPage,
                totalPages = articlesFromRepo.TotalPages
            };

            Response.Headers.Add("x-pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return searchParams.Keyword == null || searchParams.Keyword == string.Empty ? Ok(articlesFromRepo)
                : Ok(articlesFromRepo.ShapeData(searchParams.Fields));
        }

        [HttpGet("searchByCategory", Name = "GetArticlesByCategory")]
        public async Task<IActionResult> GetArticlesByCategory(
            [FromQuery] ArticleResourceParameters searchParams,
            [FromQuery] PaginationResourceParameters paginationParams
            )
        {
            if (string.IsNullOrEmpty(searchParams.Category)) return BadRequest("Category cannot be null or empty");

            var articlesFromRepo = await _articleRepository.GetArticlesByCategoryAsync(
                searchParams.Category,
                paginationParams.PageSize,
                paginationParams.PageNumber);


            if (articlesFromRepo == null || !articlesFromRepo.Any())
            {
                return NotFound("No articles found.");
            }

            var previousPageLink = articlesFromRepo.HasPrevious
                ? GenerateResourceURL(
                    paginationParams, ResourceUriType.PreviousPage, "GetArticlesByCategory",
                    new Dictionary<string, object> { { "category", searchParams.Category } })
                : null;

            var nextPageLink = articlesFromRepo.HasNext
                ? GenerateResourceURL(
                    paginationParams, ResourceUriType.NextPage, "GetArticlesByCategory",
                    new Dictionary<string, object> { { "category", searchParams.Category } })
                : null;

            var paginationMetadata = new
            {
                previousPageLink,
                nextPageLink,
                totalCount = articlesFromRepo.TotalCount,
                pageSize = articlesFromRepo.PageSize,
                currentPage = articlesFromRepo.CurrentPage,
                totalPages = articlesFromRepo.TotalPages
            };

            Response.Headers.Add("x-pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(articlesFromRepo.ShapeData(searchParams.Fields));
        }

        [HttpGet("searchByTags", Name = "GetArticlesByTags")]
        public async Task<IActionResult> GetArticlesByTags(
            [FromQuery] ArticleResourceParameters searchParams,
            [FromQuery] PaginationResourceParameters paginationParams
            )
        {
            if (string.IsNullOrEmpty(searchParams.Tags)) return BadRequest("Tags cannot be null or empty");

            var tagNames = searchParams.Tags.Split(',').ToList();

            var articlesFromRepo = await _articleRepository.GetArticlesByTagsAsync(
                tagNames,
                paginationParams.PageSize,
                paginationParams.PageNumber);


            if (articlesFromRepo == null || !articlesFromRepo.Any())
            {
                return NotFound("No articles found.");
            }

            var previousPageLink = articlesFromRepo.HasPrevious
                ? GenerateResourceURL(
                    paginationParams, ResourceUriType.PreviousPage, "GetArticlesByTags",
                    new Dictionary<string, object> { { "Tags", searchParams.Tags } })
                : null;

            var nextPageLink = articlesFromRepo.HasNext
                ? GenerateResourceURL(
                    paginationParams, ResourceUriType.NextPage, "GetArticlesByTags",
                    new Dictionary<string, object> { { "Tags", searchParams.Tags } })
                : null;

            var paginationMetadata = new
            {
                previousPageLink,
                nextPageLink,
                totalCount = articlesFromRepo.TotalCount,
                pageSize = articlesFromRepo.PageSize,
                currentPage = articlesFromRepo.CurrentPage,
                totalPages = articlesFromRepo.TotalPages
            };

            Response.Headers.Add("x-pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

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

