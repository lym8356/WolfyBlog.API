using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WolfyBlog.API.Entities
{
    public class Album
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(1500)]
        public string Description { get; set; }
        [Required]
        public string Path { get; set; }
        public string Cover { get; set; }
        public ICollection<Photo> AlbumPhotos { get; set; } = new List<Photo>();
    }
}

