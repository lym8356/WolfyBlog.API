using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WolfyBlog.API.Database;
using WolfyBlog.API.DTOs;
using WolfyBlog.API.Entities;

namespace WolfyBlog.API.Services
{
	public class AlbumRepository : IAlbumRepository
	{

        private readonly DataContext _context;
        private readonly IMapper _mapper;
        

        public AlbumRepository(DataContext context, IMapper mapper)
		{
            _mapper = mapper;
            _context = context;
        }

        public async Task<bool> AlbumExistsAsync(int albumId)
        {
            return await _context.Albums.AnyAsync(a => a.Id == albumId);
        }

        public async Task<bool> AlbumExistsAsync(string albumTitle)
        {
            return await _context.Albums.AnyAsync(a => a.Title == albumTitle);
        }

        public async Task<Album> GetAlbumAsync(int albumId)
        {
            return await _context.Albums.Include(a => a.AlbumPhotos).FirstOrDefaultAsync(a => a.Id == albumId);
        }

        public async Task<IEnumerable<Album>> GetAlbumsAsync()
        {
            return await _context.Albums.Include(a => a.AlbumPhotos).ToListAsync();
        }

        public Album CreateAlbum(AlbumForCreationDTO albumForCreationDTO, string path)
        {
            var album = _mapper.Map<Album>(albumForCreationDTO);
            album.Path = path;
                      
            _context.Albums.Add(album);
            return album;
        }

        public Album EditAlbum(Album albumFromRepo, AlbumForUpdateDTO albumForUpdateDTO)
        {
            _mapper.Map(albumForUpdateDTO, albumFromRepo);
            _context.Update(albumFromRepo);
            return albumFromRepo;
        }

        public void DeleteAlbum(Album album)
        {
            _context.Albums.Remove(album);
        }

        
        public async Task<bool> ImageExistAsync(Guid imageId)
        {
            return await _context.Photos.AnyAsync(p => p.Id == imageId);
        }

        public async Task<Photo> GetImageAsync(Guid imageId)
        {
            return await _context.Photos.FirstOrDefaultAsync(p => p.Id == imageId);
        }

        public void AddImage(Photo image, Album album)
        {
            // set the first image in the album as cover
            if (album.AlbumPhotos.Count() < 1)
            {
                album.Cover = image.Url;
            }
            // add photo into the album
            album.AlbumPhotos.Add(image);
            // save photo objects
            _context.Photos.Add(image);
        }

        public void EditImage(Photo imageFromRepo, string newPublicId, string newUrl)
        {
            imageFromRepo.publicId = newPublicId;
            imageFromRepo.Url = newUrl;
            _context.Update(imageFromRepo);
        }

        public void DeleteImage(Photo image)
        {
            _context.Photos.Remove(image);
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }

        public void setCover(Album album, string imgUrl)
        {
            album.Cover = imgUrl;
            _context.Update(album);
        }
    }
}

