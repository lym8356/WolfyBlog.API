using System;
using System.ComponentModel.DataAnnotations;

namespace WolfyBlog.API.DTOs
{
	public class SiteLogForCreationDTO
	{
        [Required]
        [MaxLength(1500)]
        public string Description { get; set; }
        [Required]
        public DateTime DateAdded { get; set; }
    }
}

