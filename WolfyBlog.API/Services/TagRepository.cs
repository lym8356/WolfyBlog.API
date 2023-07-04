using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WolfyBlog.API.Database;
using WolfyBlog.API.DTOs;
using WolfyBlog.API.Entities;
using AutoMapper.QueryableExtensions;

namespace WolfyBlog.API.Services
{
    public class TagRepository : ITagRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public TagRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

        public async Task<IEnumerable<TagDTO>> GetTagsAsync()
        {
            return await _context.Tags
                .ProjectTo<TagDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }

        public async Task<bool> TagExistsAsync(int tagId)
        {
            return await _context.Tags.AnyAsync(c => c.Id == tagId);
        }

        // count how many of the tags in the tagNames list exist in the database
        public async Task<bool> TagExistsAsync(List<string> tagNames)
        {
            int existingTagCount = await _context.Tags.CountAsync(t => tagNames.Contains(t.Title));
            return existingTagCount == tagNames.Count;
        }
    }
}

