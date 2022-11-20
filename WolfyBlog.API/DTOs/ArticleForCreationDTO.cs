using System;
using System.ComponentModel.DataAnnotations;
using WolfyBlog.API.Entities;

namespace WolfyBlog.API.DTOs
{
    public class ArticleForCreationDTO
    {
        [Required(ErrorMessage = "Title cannot be null.")]
        [MaxLength(100, ErrorMessage = "Title must be less than 100 characters.")]
        public string Title { get; set; }
        public string? Content { get; set; }
        [Required(ErrorMessage = "Category is required.")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "At least one tag needs to be selected.")]
        public int[] TagIds { get; set; }
        [Required(ErrorMessage = "Article type is required.")]
        public bool IsDraft { get; set; }
    }
}

