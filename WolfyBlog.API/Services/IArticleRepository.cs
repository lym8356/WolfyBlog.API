
using System;
using WolfyBlog.API.DTOs;
using WolfyBlog.API.Entities;

namespace WolfyBlog.API.Services
{
    public interface IArticleRepository
    {
        Task<IEnumerable<ArticleDTO>> GetArticlesAsync();
        Task<Article> GetArticleAsync(Guid articleId);
        Task<IEnumerable<Article>> GetArticlesByCategoryAsync();
        Task<IEnumerable<Article>> GetArticlesByTagsAsync();
        Task CreateArticleAsync(ArticleForCreationDTO articleForCreationDTO);
        Task EditArticleAsync(Article articleFromRepo, ArticleForUpdateDTO articleForUpdateDTO);
        void DeleteArticleAsync(Article article);
        Task<bool> ArticleExistsAsync(Guid articleId);
        Task<bool> SaveAsync();
    }
}

