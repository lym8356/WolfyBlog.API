using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WolfyBlog.API.Database;
using WolfyBlog.API.DTOs;
using WolfyBlog.API.Entities;
using WolfyBlog.API.Helper;
using WolfyBlog.API.Services.Interfaces;

namespace WolfyBlog.API.Services
{
	public class CommentRepository : ICommentRepository
	{
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CommentRepository(DataContext context, IMapper mapper)
		{
            _mapper = mapper;
            _context = context;
        }

        public async Task<bool> CommentExistAsync(Guid id)
        {
            return await _context.Comments.AnyAsync(c => c.Id == id);
        }

        public void CreateComment(Comment comment)
        {
            _context.Comments.Add(comment);
        }

        public void DeleteComment(Comment comment)
        {
            _context.Comments.Remove(comment);
        }
            
        public async Task<Comment> FindCommentById(Guid id)
        {
            return await _context.Comments.FindAsync(id);
        }

        public async Task<Comment> GetCommentAsync(Guid id)
        {
            //return await _context.Comments
            //    .ProjectTo<Comment>(_mapper.ConfigurationProvider)
            //    .FirstOrDefaultAsync(c => c.Id == id);
            throw new NotImplementedException();
        }

        public async Task<PaginationList<Comment>> GetCommentsAsync(
            int? pageSize,
            int? pageNumber)
        {
            IQueryable<Comment> result = _context.Comments.OrderByDescending(c => c.CreatedAt);

            if (pageSize.HasValue && pageNumber.HasValue)
            {
                return await PaginationList<Comment>.CreateAsync(pageNumber.Value, pageSize.Value, result);
            }
            else
            {
                var totalCount = await result.CountAsync();
                var items = await result.ToListAsync();

                return new PaginationList<Comment>(totalCount, 1, totalCount, items);
            }
        }

        public async Task<PaginationList<Comment>> GetDiscussionCommentsAsync(
            int? pageSize,
            int? pageNumber)
        {
            IQueryable<Comment> result = _context.Comments
                .Where(c => c.ReplyToArticleId == null && c.ParentCommentId == null)
                .Include(c => c.Replies)
                .OrderByDescending(c => c.CreatedAt);

            if (pageSize.HasValue && pageNumber.HasValue)
            {
                return await PaginationList<Comment>.CreateAsync(pageNumber.Value, pageSize.Value, result);
            }
            else
            {
                var totalCount = await result.CountAsync();
                var items = await result.ToListAsync();

                return new PaginationList<Comment>(totalCount, 1, totalCount, items);
            }
        }

        public async Task<IEnumerable<Comment>> GetCommentsByArticleId(Guid articleId)
        {
            return  await _context.Comments.Where(c => c.ReplyToArticle.Id == articleId).ToListAsync();
        }

        public Comment MapToComment(CommentForCreationDTO commentForCreationDTO)
        {
            var commentToReturn = _mapper.Map<Comment>(commentForCreationDTO);
            return commentToReturn;
        }

        public async Task RemoveReplies(Guid parentId)
        {
            var repliesToRemove = await _context.Comments.Where(c => c.ParentCommentId == parentId).ToListAsync();
            foreach(var reply in repliesToRemove)
            {
                await RemoveReplies(reply.Id);
                DeleteComment(reply);
            }
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}

