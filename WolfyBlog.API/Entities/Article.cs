using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WolfyBlog.API.Entities
{
    public class Article
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength]
        public string? Content { get; set; }
        public Category? Category { get; set; }
        public ICollection<ArticleTag> ArticleTags { get; set; } = new List<ArticleTag>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        [Required]
        public bool IsDraft { get; set; }
    }
}

