using System;
using WolfyBlog.API.Entities;

namespace WolfyBlog.API.DTOs
{
    public class ArticleDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public CategoryDTO Category { get; set; }
        public ICollection<TagDTO> ArticleTags { get; set; }
        public bool IsDraft { get; set; }
    }
}

