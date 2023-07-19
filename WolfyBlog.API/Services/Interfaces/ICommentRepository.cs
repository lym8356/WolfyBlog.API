using System;
using WolfyBlog.API.DTOs;
using WolfyBlog.API.Entities;
using WolfyBlog.API.Helper;

namespace WolfyBlog.API.Services.Interfaces
{
	public interface ICommentRepository
	{
        Task<bool> CommentExistAsync(Guid id);
        Task<Comment> GetCommentAsync(Guid id);
        Task<PaginationList<Comment>> GetCommentsAsync(int? pageSize, int? pageNumber);
        Task RemoveReplies(Guid parentId);
        Task<Comment> FindCommentById(Guid id);
        Comment MapToComment(CommentForCreationDTO commentForCreationDTO);
        void CreateComment(Comment comment);
        void DeleteComment(Comment comment);
        Task<IEnumerable<Comment>> GetCommentsByArticleId(Guid articleId);
        Task<PaginationList<Comment>> GetDiscussionCommentsAsync(int? pageSize, int? pageNumber);
        Task<bool> SaveAsync();
    }
}

