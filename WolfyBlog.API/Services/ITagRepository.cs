using System;
using WolfyBlog.API.Entities;

namespace WolfyBlog.API.Services
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetTagsAsync();
        Task<Tag> GetTagAsync(int tagId);
        void CreateTagAsync(Tag tag);
        void DeleteTagAsync(Tag tag);
        Task<bool> TagExistsAsync(int tagId);
        Task<bool> SaveAsync();
    }
}

