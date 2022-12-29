using System;
using WolfyBlog.API.DTOs;
using WolfyBlog.API.Entities;

namespace WolfyBlog.API.Services
{
    public interface IArticleRepository
    {
        Task<IEnumerable<ArticleDTO>> GetArticlesAsync();
        Task<ArticleDTO> GetArticleAsync(Guid articleId);
        Task<Article> FindArticleByIdAsync(Guid articleId);
        Task<IEnumerable<ArticleDTO>> GetArticlesByCategoryAsync();
        Task<IEnumerable<ArticleDTO>> GetArticlesByTagsAsync();
        Task<ArticleDTO> CreateArticleAsync(ArticleForCreationDTO articleForCreationDTO);
        Task<ArticleDTO> EditArticleAsync(Article articleFromRepo, ArticleForUpdateDTO articleForUpdateDTO);
        void DeleteArticle(Article article);
        Task<bool> ArticleExistsAsync(Guid articleId);
        Task<bool> SaveAsync();
    }
}

