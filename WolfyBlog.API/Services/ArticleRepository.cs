using Microsoft.EntityFrameworkCore;
using WolfyBlog.API.Database;
using WolfyBlog.API.Entities;
using AutoMapper.QueryableExtensions;
using WolfyBlog.API.DTOs;
using AutoMapper;

namespace WolfyBlog.API.Services
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public ArticleRepository(DataContext context, IMapper mapper) 
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<bool> ArticleExistsAsync(Guid articleId)
        {
            return await _context.Articles.AnyAsync(a => a.Id == articleId);
        }

        public async Task<ArticleDTO> CreateArticleAsync(ArticleForCreationDTO articleForCreationDTO)
        {
            var articleToSave = _mapper.Map<Article>(articleForCreationDTO);
            var category = await _context.Categories.FindAsync(articleForCreationDTO.CategoryId);
            // handle add category
            if (category != null)
            {
                articleToSave.Category = category;
            }
            // handle add tag
            foreach(var tagId in articleForCreationDTO.TagIds)
            {
                var tag = await _context.Tags.FindAsync(tagId);
                if (tag != null)
                {
                    articleToSave.ArticleTags.Add(new ArticleTag { Article = articleToSave, Tag = tag });
                }
            }
            _context.Articles.Add(articleToSave);
            // map to articleDTO to avoid object cycle
            var articleToReturn = _mapper.Map<ArticleDTO>(articleToSave);
            return articleToReturn;
        }

        public void DeleteArticle(Article article)
        {
            _context.Articles.Remove(article);
        }

        public async Task<ArticleDTO> EditArticleAsync(Article articleFromRepo, ArticleForUpdateDTO articleForUpdateDTO)
        {
            _mapper.Map(articleForUpdateDTO, articleFromRepo);
            // clear old category and tags
            articleFromRepo.Category = null;
            articleFromRepo.ArticleTags.Clear();

            // clear join table data
            var articleTagsToRemove = _context.ArticleTags.Where(a => a.ArticleId == articleFromRepo.Id);
            _context.RemoveRange(articleTagsToRemove);

            var category = await _context.Categories.FindAsync(articleForUpdateDTO.CategoryId);
            // handle add category
            if (category != null)
            {
                articleFromRepo.Category = category;
            }
            // handle add tag
            foreach (var tagId in articleForUpdateDTO.TagIds)
            {
                var tag = await _context.Tags.FindAsync(tagId);
                if (tag != null)
                {
                    articleFromRepo.ArticleTags.Add(new ArticleTag { Article = articleFromRepo, Tag = tag });
                }
            }
            _context.Update(articleFromRepo);
            var articleToReturn = _mapper.Map<ArticleDTO>(articleFromRepo);
            return articleToReturn;
        }

        public async Task<ArticleDTO> GetArticleAsync(Guid articleId)
        {
            return await _context.Articles
                .ProjectTo<ArticleDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(a => a.Id == articleId);
        }

        public async Task<Article> FindArticleByIdAsync(Guid articleId)
        {
            return await _context.Articles.FindAsync(articleId);
        }

        public async Task<IEnumerable<ArticleDTO>> GetArticlesAsync()
        {
            return await _context.Articles
                .ProjectTo<ArticleDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public Task<IEnumerable<ArticleDTO>> GetArticlesByCategoryAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ArticleDTO>> GetArticlesByTagsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}

