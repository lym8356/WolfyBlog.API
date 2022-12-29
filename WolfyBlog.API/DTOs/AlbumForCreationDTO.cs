using System;
using System.ComponentModel.DataAnnotations;

namespace WolfyBlog.API.DTOs
{
	public class AlbumForCreationDTO
	{
        [Required(ErrorMessage = "Title cannot be null.")]
        [MaxLength(100, ErrorMessage = "Title must be less than 100 characters.")]
        public string Title { get; set; }
        [Required]
        [MaxLength(1500, ErrorMessage = "Title must be less than 1500 characters.")]
        public string Description { get; set; }
    }
}

