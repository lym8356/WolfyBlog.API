using System;
using WolfyBlog.API.DTOs;
using WolfyBlog.API.Entities;

namespace WolfyBlog.API.Services
{
	public interface IAlbumRepository
	{
        Task<IEnumerable<Album>> GetAlbumsAsync();
        Task<Album> GetAlbumAsync(int id);
        Album CreateAlbum(AlbumForCreationDTO albumForCreationDTO, string path);
        Album EditAlbum(Album albumFromRepo, AlbumForUpdateDTO albumForUpdateDTO);
		void DeleteAlbum(Album album);
        Task<bool> AlbumExistsAsync(int albumId);
        Task<bool> AlbumExistsAsync(string albumTitle);

        Task<Photo> GetImageAsync(Guid imageId);
        void AddImage(Photo image, Album album);
        void EditImage(Photo imageFromRepo, string newPublicId, string newUrl);
        void DeleteImage(Photo image);
        void setCover(Album album, string newUrl);
        Task<bool> ImageExistAsync(Guid imageId);
        Task<bool> SaveAsync(); 
    }
}

