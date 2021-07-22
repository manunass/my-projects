using GreenCoinService.Services.Interfaces;
using GreenCoinService.Stores.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GreenCoinService.Services
{
    public class ImagesService : IImagesService
    {
        private readonly IImagesStore _imageStore;

        public ImagesService(IImagesStore imageStore)
        {
            _imageStore = imageStore;
        }

        public async Task UploadImages(string userId, List<IFormFile> files)
        {
            var tasks = new Queue<Task>();
            foreach(var file in files)
            {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                tasks.Enqueue(_imageStore.Upload(userId, stream.ToArray(), file.ContentType));
            }
            await Task.WhenAll(tasks);
        }

        public async Task<List<string>> ListImagesUris(string userId)
        {
            var imagesUrls = await _imageStore.ListUris(userId);
            return imagesUrls;
        }

        public async Task DeleteImage(string userId, string imageId)
        {
            await _imageStore.Delete(userId, imageId);
        }

        public async Task DeleteImagesFolder(string userId)
        {
            await _imageStore.DeleteFolder(userId);
        }
    }
}
