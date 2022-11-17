using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WolfyBlog.API.Database;
using WolfyBlog.API.Entities;

namespace WolfyBlog.API.Services
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;
        public CategoryRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CategoryExistsAsync(int categoryId)
        {
            return await _context.Categories.AnyAsync(c => c.Id == categoryId);
        }

        public void CreateCategoryAsync(Category category)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));

            _context.Categories.Add(category);
        }

        public void DeleteCategoryAsync(Category category)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));
            _context.Categories.Remove(category);
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryAsync(int categoryId)
        {
            return await _context.Categories.FindAsync(categoryId);
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}

