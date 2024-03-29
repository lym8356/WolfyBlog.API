﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WolfyBlog.API.Entities
{
    public class AboutPage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength]
        public string Content { get; set; }
        [Required]
        public bool IsAboutSite { get; set; }
        [Required]
        public bool IsNotification { get; set; }
    }
}

