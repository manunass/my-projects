using System.Threading.Tasks;

namespace Aub.Eece503e.ChatService.Web.Store
{
    public interface IImageStore
    {
        Task<string> Upload(byte[] bytes);
        Task<byte[]> Download(string id);
        Task Delete(string id);

    }
}
