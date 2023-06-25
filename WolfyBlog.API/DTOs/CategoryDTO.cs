using WolfyBlog.API.Entities;

namespace WolfyBlog.API.DTOs
{
	public class CategoryDTO
	{
        public int Id { get; set; }
        public string Title { get; set; }
        public int ArticleCount { get; set; }
    }
}

