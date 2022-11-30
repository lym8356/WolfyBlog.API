using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WolfyBlog.API.Entities;
using WolfyBlog.API.Services;

namespace WolfyBlog.API.Controllers
{
    [Route("api/[controller]")] // api/Category
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categoriesFromRepo = await _categoryRepository.GetCategoriesAsync();
            if (categoriesFromRepo == null || categoriesFromRepo.Count() <= 0)
            {
                return NotFound("No categories found.");
            }
            return Ok(categoriesFromRepo);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            _categoryRepository.CreateCategoryAsync(category);
            var result = await _categoryRepository.SaveAsync();
            if (result) return Ok(category);
            return BadRequest(new ProblemDetails { Title = "Problem creating category" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{categoryId}")]
        public async Task<IActionResult> EditCategory(int categoryId, Category category)
        {
            if (!(await _categoryRepository.CategoryExistsAsync(categoryId)))
            {
                return NotFound("Category does not exist.");
            }

            var categoryFromRepo = await _categoryRepository.GetCategoryAsync(categoryId);

            categoryFromRepo.Title = category.Title;
            var result = await _categoryRepository.SaveAsync();
            if (result) return Ok();
            return BadRequest(new ProblemDetails { Title = "Problem editing category" });

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            if (!(await _categoryRepository.CategoryExistsAsync(categoryId)))
            {
                return NotFound("Category does not exist.");
            }

            var categoryFromRepo = await _categoryRepository.GetCategoryAsync(categoryId);
            _categoryRepository.DeleteCategoryAsync(categoryFromRepo);
            var result = await _categoryRepository.SaveAsync();
            if (result) return Ok();
            return BadRequest(new ProblemDetails { Title = "Problem deleting category" });
        }
    }
}

