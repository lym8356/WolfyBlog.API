using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WolfyBlog.API.Entities
{
    public class SiteLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(1500)]
        public string Description { get; set; }
        [Required]
        public DateTime DatedAdded { get; set; }
    }
}

