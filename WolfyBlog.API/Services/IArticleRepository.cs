
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
        Task CreateArticleAsync(ArticleForCreationDTO articleForCreationDTO);
        Task EditArticleAsync(Article articleFromRepo, ArticleForUpdateDTO articleForUpdateDTO);
        void DeleteArticleAsync(Article article);
        Task<bool> ArticleExistsAsync(Guid articleId);
        Task<bool> SaveAsync();
    }
}

