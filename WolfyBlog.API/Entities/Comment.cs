using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WolfyBlog.API.Entities
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(2000)]
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Required]
        [MaxLength(20)]
        public string CommenterUsername { get; set; }
        [Required]
        [EmailAddress]
        public string CommenterEmail { get; set; }
        public Guid? ParentCommentId { get; set; }
        [JsonIgnore]
        public Comment? ParentComment { get; set; }
        public Guid? ReplyToArticleId { get; set; }
        [JsonIgnore]
        public Article? ReplyToArticle { get; set; }
        public ICollection<Comment> Replies { get; set; } = new List<Comment>();
    }
}

