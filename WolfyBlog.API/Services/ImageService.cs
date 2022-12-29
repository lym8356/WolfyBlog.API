using System;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using WolfyBlog.API.Entities;
using static System.Net.Mime.MediaTypeNames;

namespace WolfyBlog.API.Services
{
	public class ImageService
	{
		private readonly Cloudinary _cloudinary;
		public ImageService(IConfiguration config)
		{
			var acc = new Account(
				config["Cloudinary:CloudName"],
				config["Cloudinary:ApiKey"],
				config["Cloudinary:ApiSecret"]
			);

			_cloudinary = new Cloudinary(acc);
		}

		public async Task<CreateFolderResult> CreateFolder(string albumName)
		{
			var result = new CreateFolderResult();
            result = await _cloudinary.CreateFolderAsync($"/WolfyBlog/{albumName}");
            return result;
		}

        // change the public id of all images in the old folder to match the new folder name
        // https://support.cloudinary.com/hc/en-us/articles/206347109

        public async Task<RenameResult> EditPublicId(Photo image, string newAlbumName)
		{
            // extract the id of the image, this will be the file name
            var imageId = image.publicId.Substring(image.publicId.LastIndexOf("/") + 1);
            // change the public ID of these photos to match the new folder name
            var renameParams = new RenameParams(image.publicId, $"WolfyBlog/{newAlbumName}/{imageId}");
            var renameResult = await _cloudinary.RenameAsync(renameParams);

            return renameResult;
		}

		public async Task<DeleteFolderResult> DeleteFolder(string albumPath)
		{
			var result = new DeleteFolderResult();
			result = await _cloudinary.DeleteFolderAsync(albumPath);
			return result;
		}

		// maximum file size 10MB
		public async Task<ImageUploadResult> AddImageAsync(IFormFile file, string albumPath)
		{
			var uploadResult = new ImageUploadResult();

			if (file.Length > 0)
			{
				using var stream = file.OpenReadStream();
				var uploadParams = new ImageUploadParams
				{
					File = new FileDescription(file.FileName, stream),
					//Transformation = new Transformation().Height(1000).Width(1000).Crop("fill"),
					Folder = albumPath
				}; 
				uploadResult = await _cloudinary.UploadAsync(uploadParams);
                return uploadResult;
            }
			return null;
		}

		public async Task<DeletionResult> DeleteImage(string publicId)
		{
			var deleteParams = new DeletionParams(publicId);
			var result = await _cloudinary.DestroyAsync(deleteParams);
			return result;
		}
	}
}

