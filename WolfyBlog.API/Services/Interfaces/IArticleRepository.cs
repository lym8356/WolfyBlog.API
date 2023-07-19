using WolfyBlog.API.DTOs;
using WolfyBlog.API.Entities;
using WolfyBlog.API.Helper;

namespace WolfyBlog.API.Services
{
    public interface IArticleRepository
    {
        Task<PaginationList<ArticleDTO>> GetArticlesAsync(
            string keyword, int? pageSize, int? pageNumber);
        Task<ArticleDTO> GetArticleAsync(Guid articleId);
        Task<ArticleDTO> GetArticleAsync(string articleSlug);
        Task<Article> FindArticleByIdAsync(Guid articleId);
        Task<PaginationList<ArticleDTO>> GetArticlesByCategoryAsync(
            string categoryName,
            int? pageSize, int? pageNumber);
        Task<PaginationList<ArticleDTO>> GetArticlesByTagsAsync(
            IEnumerable<string> tagNames,
            int? pageSize, int? pageNumber);
        Task<ArticleDTO> CreateArticleAsync(ArticleForCreationDTO articleForCreationDTO);
        Task<ArticleDTO> EditArticleAsync(Article articleFromRepo, ArticleForUpdateDTO articleForUpdateDTO);
        void DeleteArticle(Article article);
        Task<bool> ArticleExistsAsync(Guid articleId);
        Task<bool> ArticleExistsAsync(string articleSlug);
        Task<bool> SaveAsync();
    }
}

