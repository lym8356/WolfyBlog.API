using Microsoft.EntityFrameworkCore;
using WolfyBlog.API.Database;
using WolfyBlog.API.Entities;
using AutoMapper.QueryableExtensions;
using WolfyBlog.API.DTOs;
using AutoMapper;
using WolfyBlog.API.Helper;

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

        public async Task<PaginationList<ArticleDTO>> GetArticlesAsync(
            string keyword,
            int? pageSize,
            int? pageNumber)
        {
            IQueryable<ArticleDTO> result = _context.Articles.ProjectTo<ArticleDTO>(_mapper.ConfigurationProvider)
                .OrderByDescending(a => a.CreatedAt);
                
            if(!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();
                result = result.Where(a => a.Title.Contains(keyword));
            }

            if (pageSize.HasValue && pageNumber.HasValue)
            {
                return await PaginationList<ArticleDTO>.CreateAsync(pageNumber.Value, pageSize.Value, result);
            } else
            {
                var totalCount = await result.CountAsync();
                var items = await result.ToListAsync();

                return new PaginationList<ArticleDTO>(totalCount, 1, totalCount, items);
            }


            //return await result.ToListAsync();
            //return await _context.Articles
            //    .ProjectTo<Article>(_mapper.ConfigurationProvider)
            //    .ToListAsync();
        }

        public async Task<PaginationList<ArticleDTO>> GetArticlesByCategoryAsync(string categoryName,
            int? pageSize,
            int? pageNumber)
        {
            var result = _context.Articles.AsQueryable().ProjectTo<ArticleDTO>(_mapper.ConfigurationProvider);

            if (!string.IsNullOrEmpty(categoryName))
            {
                result = result.Where(a => a.Category.Title == categoryName);
            }

            if (pageSize.HasValue && pageNumber.HasValue)
            {
                return await PaginationList<ArticleDTO>.CreateAsync(pageNumber.Value, pageSize.Value, result);
            }
            else
            {
                var totalCount = await result.CountAsync();
                var items = await result.ToListAsync();

                return new PaginationList<ArticleDTO>(totalCount, 1, totalCount, items);
            }

        }

        public async Task<PaginationList<ArticleDTO>> GetArticlesByTagsAsync(
            IEnumerable<string> tagNames,
            int? pageSize, int? pageNumber)
        {
            var articles = _context.Articles.AsQueryable();
            if (tagNames != null && tagNames.Any())
            {
                foreach (var tagName in tagNames)
                {
                    var currentTagName = tagName; // to avoid access to modified closure
                    articles = articles.Where(a => a.ArticleTags.Any(at => at.Tag.Title == currentTagName));
                }
            }

            var result = articles.ProjectTo<ArticleDTO>(_mapper.ConfigurationProvider);

            if (pageSize.HasValue && pageNumber.HasValue)
            {
                return await PaginationList<ArticleDTO>.CreateAsync(pageNumber.Value, pageSize.Value, result);
            }
            else
            {
                var totalCount = await result.CountAsync();
                var items = await result.ToListAsync();

                return new PaginationList<ArticleDTO>(totalCount, 1, totalCount, items);
            }

            //return await _context.Articles
            //    .Where(a => tagNames.All(tag => a.ArticleTags.Any(t => t.Tag.Title == tag)))
            //    .ProjectTo<ArticleDTO>(_mapper.ConfigurationProvider)
            //    .ToListAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}

