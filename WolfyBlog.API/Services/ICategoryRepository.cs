using WolfyBlog.API.DTOs;
using WolfyBlog.API.Entities;

namespace WolfyBlog.API.Services
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryDTO>> GetCategoriesAsync();
        Task<Category> GetCategoryAsync(int categoryId);
        void CreateCategoryAsync(Category category);
        void DeleteCategoryAsync(Category category);
        Task<bool> CategoryExistsAsync(int categoryId);
        Task<bool> SaveAsync();
    }
}

