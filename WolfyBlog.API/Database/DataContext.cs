using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WolfyBlog.API.Entities;

namespace WolfyBlog.API.Database
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        DbSet<Article> Articles { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<Tag> Tags { get; set; }
        DbSet<ArticleTag> ArticleTags { get; set; }
        DbSet<AboutPage> AboutPage { get; set; }
        DbSet<Project> Projects { get; set; }
        DbSet<SiteLog> SiteLogs { get; set; }
        DbSet<Album> Albums { get; set; }
        DbSet<Photo> Photos { get; set; }
        
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
        }
    }
}

