using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WolfyBlog.API.DTOs;
using WolfyBlog.API.Entities;
using WolfyBlog.API.Services;

namespace WolfyBlog.API.Controllers
{
    [Route("api/[controller]")] // api/Album
    [ApiController]
    public class AlbumController : ControllerBase
	{
		private IAlbumRepository _albumRepository;
        private readonly ImageService _imageService;
        public AlbumController(IAlbumRepository albumRepository, ImageService imageService)
		{
			_albumRepository = albumRepository;
			_imageService = imageService;
		}

        [HttpGet]
        public async Task<IActionResult> GetAlbums()
        {
            var albumsFromRepo = await _albumRepository.GetAlbumsAsync();
            if (albumsFromRepo == null || albumsFromRepo.Count() <= 0)
            {
                return NotFound("No albums found.");
            }
            return Ok(albumsFromRepo);
        }

        [HttpGet("{albumId}", Name = "GetAlbum")]
        public async Task<IActionResult> GetAlbum(int albumId)
        {
            if (!(await _albumRepository.AlbumExistsAsync(albumId)))
            {
                return NotFound("Album does not exist.");
            }
            var albumFromRepo = await _albumRepository.GetAlbumAsync(albumId);
            return Ok(albumFromRepo);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateAlbum([FromBody] AlbumForCreationDTO albumForCreationDTO)
        {
            // check for duplicate album name
            if (await _albumRepository.AlbumExistsAsync(albumForCreationDTO.Title))
            {
                return BadRequest("Album name already exists");
            }
            var createResult = _imageService.CreateFolder(albumForCreationDTO.Title).Result;
            if (createResult.Error != null)
            {
                return BadRequest(new ProblemDetails { Title = createResult.Error.Message });
            }
            var albumToReturn = _albumRepository.CreateAlbum(albumForCreationDTO, createResult.Path);
            var result = await _albumRepository.SaveAsync();
            if (result) return CreatedAtRoute("GetAlbum", new { AlbumId = albumToReturn.Id }, albumToReturn);
            return BadRequest(new ProblemDetails { Title = "Problem creating album" });
        }

        // currently cloudinary does not support folder renaming
        // renaming can be done by 
        // 1.Change public Id of the images in the folder (which automatically creates a new folder).
        // 2.Delete everything in the folder
        // 3.Delete the empty folder
        // avoid using this method too often
        [Authorize(Roles = "Admin")]
        [HttpPut("{albumId}")]
        public async Task<IActionResult> EditAlbum(int albumId,
            [FromBody] AlbumForUpdateDTO albumForUpdateDTO)
        {
            // check for duplicate album name
            if (await _albumRepository.AlbumExistsAsync(albumForUpdateDTO.Title))
            {
                return BadRequest("Album name already exists");
            }

            // if the album is empty, change the name and description directly
            var albumFromRepo = await _albumRepository.GetAlbumAsync(albumId);
            if (albumFromRepo == null) return NotFound("Album does not exist");
            if (albumFromRepo.AlbumPhotos.Count() < 1)
            {
                
                // delete the empty folder in cloudinary
                var deleteResult = await _imageService.DeleteFolder(albumFromRepo.Path);
                if (deleteResult.Error != null) return BadRequest(new ProblemDetails { Title = deleteResult.Error.Message });

                // create a new folder with the new name
                var createResult =  _imageService.CreateFolder(albumForUpdateDTO.Title).Result;
                if (createResult.Error != null) return BadRequest(new ProblemDetails { Title = createResult.Error.Message });
                albumFromRepo.Path = createResult.Path;
                
            } else
            {
                // album has images

                // extract the id part
                string coverImageUrl = albumFromRepo.Cover;
                int startPos = coverImageUrl.LastIndexOf("/") + 1;
                int len = coverImageUrl.LastIndexOf(".") - startPos;
                string coverImgId = coverImageUrl.Substring(startPos, len);
                
                // change the public Id of every image in the folder to match the new path
                foreach (Photo image in albumFromRepo.AlbumPhotos)
                {
                    // change public Id in cloudinary
                    var editResult = await _imageService.EditPublicId(image, albumForUpdateDTO.Title);
                    if (editResult.Error != null) return BadRequest(new ProblemDetails { Title = editResult.Error.Message });
                    // update publicId and Url in database
                    _albumRepository.EditImage(image, editResult.PublicId, editResult.Url);
                    if (editResult.PublicId.Contains(coverImgId)) coverImageUrl = image.Url;
                }
                
                // delete the empty folder
                var deleteResult = await _imageService.DeleteFolder(albumFromRepo.Path);
                if (deleteResult.Error != null) return BadRequest(new ProblemDetails { Title = deleteResult.Error.Message });
                                
                
                // update cover path and album path
                albumFromRepo.Cover = coverImageUrl;
                albumFromRepo.Path = $"WolfyBlog/{albumForUpdateDTO.Title}";
            }
            
            _albumRepository.EditAlbum(albumFromRepo, albumForUpdateDTO);
            var saveResult = await _albumRepository.SaveAsync();
            if (!saveResult) return BadRequest(new ProblemDetails { Title = "Problem updating album" });
            return Ok(albumFromRepo);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{albumId}")]
        public async Task<IActionResult> DeleteAlbum(int albumId)
        {
            
            if(!(await _albumRepository.AlbumExistsAsync(albumId)))
            {
                return NotFound("Album does not exist");
            }
            var albumToDelete = await _albumRepository.GetAlbumAsync(albumId);
            // if album contains images, delete images first
            if (albumToDelete.AlbumPhotos.Count() > 0)
            {
                foreach(Photo image in albumToDelete.AlbumPhotos)
                {
                    // delete from cloudinary
                    var deleteResult = await _imageService.DeleteImage(image.publicId);
                    if(deleteResult.Error != null)
                    {
                        return BadRequest(new ProblemDetails { Title = deleteResult.Error.Message });
                    }
                    // delete from database
                    _albumRepository.DeleteImage(image);
                }
            }

            var albumDeleteResult = _imageService.DeleteFolder(albumToDelete.Path).Result;
            if (albumDeleteResult.Error != null)
            {
                return BadRequest(new ProblemDetails { Title = albumDeleteResult.Error.Message });
            }
            // delete from database
            _albumRepository.DeleteAlbum(albumToDelete);
            var saveResult = await _albumRepository.SaveAsync();
            if (saveResult) return Ok();
            return BadRequest("Problem deleting album");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{albumId}/setCover/{imgId}")]
        public async Task<IActionResult> setCover(int albumId, Guid imgId)
        {
            if (!(await _albumRepository.AlbumExistsAsync(albumId)))
            {
                return NotFound("Album does not exist");
            }
            var albumFromRepo = await _albumRepository.GetAlbumAsync(albumId);
            if(!(await _albumRepository.ImageExistAsync(imgId)) || albumFromRepo.AlbumPhotos.Count() < 1)
            {
                return NotFound("Image does not exist or album is empty");
            }
            var imageFromRepo = await _albumRepository.GetImageAsync(imgId);
            if (!albumFromRepo.AlbumPhotos.Contains(imageFromRepo))
            {
                return BadRequest("Cannot set image from other album as cover");
            }
            _albumRepository.setCover(albumFromRepo, imageFromRepo.Url);
            var result = await _albumRepository.SaveAsync();
            if (result) return Ok(albumFromRepo);
            return BadRequest("Problem setting new cover");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{albumId}/images")]
        public async Task<IActionResult> AddImage(
            int albumId,
            [FromForm] IFormFile file)
        {
            if (!(await _albumRepository.AlbumExistsAsync(albumId)))
            {
                return NotFound("Album does not exist");
            }
            var albumFromRepo = await _albumRepository.GetAlbumAsync(albumId);
            var uploadResult = _imageService.AddImageAsync(file, albumFromRepo.Path).Result;
            if (uploadResult.Error != null)
            {
                return BadRequest(new ProblemDetails { Title = uploadResult.Error.Message });
            }

            var image = new Photo
            {
                publicId = uploadResult.PublicId,
                Url = uploadResult.SecureUrl.ToString()
            };
            _albumRepository.AddImage(image, albumFromRepo);
            var result = await _albumRepository.SaveAsync();

            if (result)
            {
                return Ok(image);
            }
            return BadRequest(new ProblemDetails { Title = "Problem adding image" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{albumId}/images/{imageId}")]
        public async Task<IActionResult> DeleteImage(Guid imageId, int albumId)
        {

            // check if album exists
            var album = await _albumRepository.GetAlbumAsync(albumId);
            if (album == null)
            {
                return NotFound("Album does not exist");
            }

            var image = await _albumRepository.GetImageAsync(imageId);
            if (image == null)
            {
                return NotFound("Image does not exist");
            }
            // delete from cloudinary
            var deleteResult = await _imageService.DeleteImage(image.publicId);
            if (deleteResult.Error != null)
            {
                return BadRequest(new ProblemDetails { Title = deleteResult.Error.Message });
            }

            
            // check if the image to be deleted is the cover
            // if yes, reset cover
            if (album.Cover == image.Url)
            {
                album.Cover = "";
            }

            // delete from database
            _albumRepository.DeleteImage(image);
            var result = await _albumRepository.SaveAsync();

            if (result)
            {
                return Ok(album);
            }
            return BadRequest(new ProblemDetails { Title = "Problem deleting image" });
        }
    }
}

