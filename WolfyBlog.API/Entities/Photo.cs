using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WolfyBlog.API.Entities
{
    public class Photo
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Url { get; set; }
    }
}