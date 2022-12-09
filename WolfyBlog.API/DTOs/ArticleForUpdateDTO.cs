using System;
namespace WolfyBlog.API.DTOs
{
    public class ArticleForUpdateDTO : ArticleForCreationDTO
    {
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

