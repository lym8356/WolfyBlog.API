using System;
using System.ComponentModel.DataAnnotations;

namespace WolfyBlog.API.DTOs
{
	public class ProjectForCreationDTO
	{
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(1500)]
        public string Description { get; set; }
        public string Link { get; set; }
        public string Cover { get; set; }
    }
}

