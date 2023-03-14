using System.ComponentModel.DataAnnotations;
using WolfyBlog.API.Entities;

namespace WolfyBlog.API.DTOs
{
	public class CommentForCreationDTO
	{
        [MaxLength(2000)]
        public string Content { get; set; }
        [Required]
        [MaxLength(20)]
        public string CommenterUsername { get; set; }
        [Required]
        [EmailAddress]
        public string CommenterEmail { get; set; }
        public Guid? ParentCommentId { get; set; }
        public Guid? ReplyToArticleId { get; set; }
    }
}

