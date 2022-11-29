using System;
using WolfyBlog.API.Entities;

namespace WolfyBlog.API.DTOs
{
    public class ArticleDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; } 
        public bool IsDraft { get; set; }
    }
}

