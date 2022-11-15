using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WolfyBlog.API.Entities
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(2000)]
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        [MaxLength(20)]
        public string CommenterUsername { get; set; }
        [Required]
        [EmailAddress]
        public string CommenterEmail { get; set; }
        public Comment? ReplyToComment { get; set; }
        public Article? ReplyToArticle { get; set; }
    }
}

