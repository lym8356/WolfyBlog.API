using System;
using WolfyBlog.API.DTOs;
using WolfyBlog.API.Entities;

namespace WolfyBlog.API.Services.Interfaces
{
	public interface ICommentRepository
	{
        Task<bool> CommentExistAsync(Guid id);
        Task<Comment> GetCommentAsync(Guid id);
        Task<IEnumerable<Comment>> GetCommentsAsync();
        Task RemoveReplies(Guid parentId);
        Task<Comment> FindCommentById(Guid id);
        Comment MapToComment(CommentForCreationDTO commentForCreationDTO);
        void CreateComment(Comment comment);
        void DeleteComment(Comment comment);
        Task<IEnumerable<Comment>> GetCommentsByArticleId(Guid articleId);
        Task<bool> SaveAsync();
    }
}

