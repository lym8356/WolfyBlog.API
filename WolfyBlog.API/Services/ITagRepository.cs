using System;
using WolfyBlog.API.DTOs;
using WolfyBlog.API.Entities;

namespace WolfyBlog.API.Services
{
    public interface ITagRepository
    {
        Task<IEnumerable<TagDTO>> GetTagsAsync();
        Task<Tag> GetTagAsync(int tagId);
        void CreateTagAsync(Tag tag);
        void DeleteTagAsync(Tag tag);
        Task<bool> TagExistsAsync(int tagId);
        Task<bool> SaveAsync();
    }
}

