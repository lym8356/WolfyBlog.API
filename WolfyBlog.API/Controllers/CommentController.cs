using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WolfyBlog.API.DTOs;
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

		public CommentController(ICommentRepository commentRepository, IArticleRepository articleRepository)
		{
            _commentRepository = commentRepository;
            _articleRepository = articleRepository;
		}

        [HttpGet]
        public async Task<IActionResult> GetComments()
        {
            var commentsFromRepo = await _commentRepository.GetCommentsAsync();
            if (commentsFromRepo == null || commentsFromRepo.Count() <= 0)
            {
                return NotFound("No comments found.");
            }
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

            if (commentForCreationDTO.ReplyToArticleId.HasValue)
            {
                var articleFromRepo = await _articleRepository
                    .FindArticleByIdAsync(commentForCreationDTO.ReplyToArticleId ?? Guid.Empty);
                if (articleFromRepo != null)
                {
                    // add the comment to the corresponding article
                    // create the comment
                    // get the article from repo and assign it to the new comment
                    // then save the comment to db
                    comment.ReplyToArticle = articleFromRepo;
                    articleFromRepo.Comments.Add(comment);
                    if (comment.ParentCommentId.HasValue)
                    {
                        // this is a reply to a article comment
                        var commentFromRepo = await _commentRepository
                            .FindCommentById(commentForCreationDTO.ParentCommentId ?? Guid.Empty);
                        if (commentFromRepo != null)
                        {
                            comment.ParentComment = commentFromRepo;
                            commentFromRepo.Replies.Add(comment);
                        }

                    }
                    _commentRepository.CreateComment(comment);
                } else
                {
                    return BadRequest(new ProblemDetails { Title = "Article does not exist" });
                }
            } else if (commentForCreationDTO.ParentCommentId.HasValue && commentForCreationDTO.ReplyToArticleId == null)
            {
                var commentFromRepo = await _commentRepository
                    .FindCommentById(commentForCreationDTO.ParentCommentId ?? Guid.Empty);
                if (commentFromRepo != null)
                {
                    // add the comment as a reply
                    // assign commentFromRepo as parent comment
                    // then save the comment to db
                    comment.ParentComment = commentFromRepo;
                    commentFromRepo.Replies.Add(comment);
                    _commentRepository.CreateComment(comment);
                } else
                {
                    return BadRequest(new ProblemDetails { Title = "ReplyToComment does not exist" });
                }
            } else
            {
                // no replyId and no articleId, this is a new comment to the comment board
                _commentRepository.CreateComment(comment);
            }
            var result = await _commentRepository.SaveAsync();
            if (result) return CreatedAtRoute("GetComment", new { CommentId = comment.Id }, comment);
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

