using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Aub.Eece503e.ChatService.Web.Services
{
    public interface IImageService
    {
        Task<string> UploadImage(IFormFile file);
        Task<byte[]> DownloadImage(string id);
        Task DeleteImage(string id);
    }
}
