using System;
using Microsoft.EntityFrameworkCore;
using WolfyBlog.API.Database;
using WolfyBlog.API.Entities;

namespace WolfyBlog.API.Services
{
    public class TagRepository : ITagRepository
    {
        private readonly DataContext _context;
        public TagRepository(DataContext context)
        {
            _context = context;
        }

        public void CreateTagAsync(Tag tag)
        {
            if (tag == null) throw new ArgumentNullException(nameof(tag));

            _context.Tags.Add(tag);
        }

        public void DeleteTagAsync(Tag tag)
        {
            if (tag == null) throw new ArgumentNullException(nameof(tag));
            _context.Tags.Remove(tag);
        }

        public async Task<Tag> GetTagAsync(int tagId)
        {
            return await _context.Tags.FindAsync(tagId);
        }

        public async Task<IEnumerable<Tag>> GetTagsAsync()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }

        public async Task<bool> TagExistsAsync(int tagId)
        {
            return await _context.Tags.AnyAsync(c => c.Id == tagId);
        }
    }
}

