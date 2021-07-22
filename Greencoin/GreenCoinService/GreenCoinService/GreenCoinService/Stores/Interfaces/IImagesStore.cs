using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenCoinService.Stores.Interfaces
{
    public interface IImagesStore
    {
        Task<string> Upload(string folderName, byte[] bytes, string contentType = null);
        Task<List<string>> ListUris(string folderName);
        Task Delete(string FolderName, string imageId);
        Task DeleteFolder(string folderId);
    }
}
