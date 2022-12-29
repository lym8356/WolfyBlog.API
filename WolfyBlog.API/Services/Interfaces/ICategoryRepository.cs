using WolfyBlog.API.DTOs;
using WolfyBlog.API.Entities;

namespace WolfyBlog.API.Services
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryDTO>> GetCategoriesAsync();
        Task<Category> GetCategoryAsync(int categoryId);
        void CreateCategory(Category category);
        void DeleteCategory(Category category);
        Task<bool> CategoryExistsAsync(int categoryId);
        Task<bool> SaveAsync();
    }
}

