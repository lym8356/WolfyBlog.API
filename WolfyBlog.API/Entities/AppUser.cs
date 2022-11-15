using System;
using Microsoft.AspNetCore.Identity;

namespace WolfyBlog.API.Entities
{
    public class AppUser : IdentityUser
    {
        public string Bio { get; set; }
        public string DisplayName { get; set; }
        public string Thumbnail { get; set; }
    }
}

