using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WolfyBlog.API.Entities;

namespace WolfyBlog.API.Database
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ArticleTag> ArticleTags { get; set; }
        public DbSet<AboutPage> AboutPage { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<SiteLog> SiteLogs { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Photo> Photos { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // configure many to many relationship for article and tag
            builder.Entity<ArticleTag>(x => x.HasKey(at => new { at.ArticleId, at.TagId }));
            builder.Entity<ArticleTag>()
                .HasOne(a => a.Article)
                .WithMany(t => t.ArticleTags)
                .HasForeignKey(aa => aa.ArticleId);

            builder.Entity<ArticleTag>()
                .HasOne(t => t.Tag)
                .WithMany(at => at.ArticleTags)
                .HasForeignKey(tt => tt.TagId);

            // delete all comments associated with one article
            builder.Entity<Comment>()
                .HasOne(a => a.ReplyToArticle)
                .WithMany(c => c.Comments)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Comment>()
                .HasOne(c => c.ReplyToComment)
                .WithOne()
                .HasForeignKey<Comment>(cm => cm.Id);

            builder.Entity<IdentityRole>()
                .HasData(
                    new IdentityRole {
                        Id = "ad9a06ac-e3bf-41cd-9949-c4d6865dc1e6",
                        Name = "Admin",
                        NormalizedName = "Admin".ToUpper()
                    },
                    new IdentityRole
                    {
                        Id = "61bc6695-86bd-42e3-88a4-1d77edf17de2",
                        Name = "Guest",
                        NormalizedName = "Guest".ToUpper()
                    }
                );
        }
    }
}

