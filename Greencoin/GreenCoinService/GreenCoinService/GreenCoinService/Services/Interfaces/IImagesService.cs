using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenCoinService.Services.Interfaces
{
    public interface IImagesService
    {
        Task UploadImages(string userId, List<IFormFile> files);
        Task<List<string>> ListImagesUris(string userId);
        Task DeleteImage(string userId, string imageId);
        Task DeleteImagesFolder(string userId);
    }
}
