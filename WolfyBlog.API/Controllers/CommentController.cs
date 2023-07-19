using System.Dynamic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using WolfyBlog.API.DTOs;
using WolfyBlog.API.Helper;
using WolfyBlog.API.ResourceParameters;
using WolfyBlog.API.Services;
using WolfyBlog.API.Services.Interfaces;

namespace WolfyBlog.API.Controllers
{
    [Route("api/[controller]")] // api/Comment
    [ApiController]
    public class CommentController : ControllerBase
    {
        private ICommentRepository _commentRepository;
        private IArticleRepository _articleRepository;
        private readonly IUrlHelper _urlHelper;

        public CommentController(
            ICommentRepository commentRepository,
            IArticleRepository articleRepository,
            IActionContextAccessor actionContextAccessor,
            IUrlHelperFactory urlHelperFactory)
        {
            _commentRepository = commentRepository;
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
            string routeName)
        {
            var queryParams = new ExpandoObject() as IDictionary<string, Object>;
            queryParams.Add("pageNumber", paginationParams.PageNumber);
            queryParams.Add("pageSize", paginationParams.PageSize);

            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    queryParams["pageNumber"] = paginationParams.PageNumber - 1;
                    break;
                case ResourceUriType.NextPage:
                    queryParams["pageNumber"] = paginationParams.PageNumber + 1;
                    break;
            }

            return _urlHelper.Link(routeName, queryParams);

        }

        [HttpGet(Name = "GetComments")]
        public async Task<IActionResult> GetComments(
            [FromQuery] PaginationResourceParameters paginationParams,
            [FromQuery] bool excludeRepliesToArticles = false)
        {

            var commentsFromRepo = excludeRepliesToArticles
        ? await _commentRepository.GetDiscussionCommentsAsync(paginationParams.PageSize, paginationParams.PageNumber)
        : await _commentRepository.GetCommentsAsync(paginationParams.PageSize, paginationParams.PageNumber);

            if (commentsFromRepo == null || !commentsFromRepo.Any())
            {
                var commentsToReturn = commentsFromRepo.ToList();
                return Ok(commentsToReturn);
            }

            var previousPageLink = commentsFromRepo.HasPrevious
                ? GenerateResourceURL(paginationParams, ResourceUriType.PreviousPage, "GetArticles")
                : null;

            var nextPageLink = commentsFromRepo.HasNext
                ? GenerateResourceURL(paginationParams, ResourceUriType.NextPage, "GetArticles")
                : null;

            var paginationMetadata = new
            {
                previousPageLink,
                nextPageLink,
                totalCount = commentsFromRepo.TotalCount,
                pageSize = commentsFromRepo.PageSize,
                currentPage = commentsFromRepo.CurrentPage,
                totalPages = commentsFromRepo.TotalPages
            };

            Response.Headers.Add("x-pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(commentsFromRepo);
        }


        [HttpGet("{commentId}", Name = "GetComment")]
        public async Task<IActionResult> GetComment(Guid commentId)
        {
            if (!(await _commentRepository.CommentExistAsync(commentId)))
            {
                return NotFound("Comment does not exist.");
            }
            var commentFromRepo = await _commentRepository.GetCommentAsync(commentId);
            return Ok(commentFromRepo);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CommentForCreationDTO commentForCreationDTO)
        {
            // map to comment entity
            var comment = _commentRepository.MapToComment(commentForCreationDTO);

            var articleFromRepo = commentForCreationDTO.ReplyToArticleId.HasValue ?
                await _articleRepository.FindArticleByIdAsync(commentForCreationDTO.ReplyToArticleId.Value) : null;

            var commentFromRepo = commentForCreationDTO.ParentCommentId.HasValue ?
                await _commentRepository.FindCommentById(commentForCreationDTO.ParentCommentId.Value) : null;
            if (commentForCreationDTO.ReplyToArticleId.HasValue)
            {
                if (articleFromRepo == null)
                {
                    return BadRequest(new ProblemDetails { Title = "Article does not exist" });
                }

                // add the comment to the corresponding article
                // create the comment
                // get the article from repo and assign it to the new comment
                // then save the comment to db
                comment.ReplyToArticle = articleFromRepo;
                articleFromRepo.Comments.Add(comment);

                if (comment.ParentCommentId.HasValue)
                {
                    // this is a reply to a article comment
                    if (commentFromRepo == null)
                    {
                        return BadRequest(new ProblemDetails { Title = "ReplyToComment does not exist" });
                    }

                    comment.ParentComment = commentFromRepo;
                    commentFromRepo.Replies.Add(comment);
                }
            }
            else if (commentForCreationDTO.ParentCommentId.HasValue && commentForCreationDTO.ReplyToArticleId == null)
            {
                if (commentFromRepo == null)
                {
                    return BadRequest(new ProblemDetails { Title = "ReplyToComment does not exist" });
                }

                // add the comment as a reply
                // assign commentFromRepo as parent comment
                // then save the comment to db
                comment.ParentComment = commentFromRepo;
                commentFromRepo.Replies.Add(comment);
            }

            _commentRepository.CreateComment(comment);

            var result = await _commentRepository.SaveAsync();

            if (result)
                return CreatedAtRoute("GetComment", new { CommentId = comment.Id }, comment);

            return BadRequest(new ProblemDetails { Title = "Problem creating comment" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(Guid commentId)
        {
            if (!(await _commentRepository.CommentExistAsync(commentId)))
            {
                return NotFound("Comment does not exist.");
            }

            var commentFromRepo = await _commentRepository.FindCommentById(commentId);
            await _commentRepository.RemoveReplies(commentFromRepo.Id);
            _commentRepository.DeleteComment(commentFromRepo);
            var result = await _commentRepository.SaveAsync();
            if (result) return Ok();
            return BadRequest(new ProblemDetails { Title = "Problem deleting comment" });
        }
    }
}
