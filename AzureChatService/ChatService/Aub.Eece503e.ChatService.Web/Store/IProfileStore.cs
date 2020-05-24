using System.Threading.Tasks;
using Aub.Eece503e.ChatService.Datacontracts;

namespace Aub.Eece503e.ChatService.Web.Store
{
    public interface IProfileStore
    {
        Task AddUser(User user);
        Task<User> GetUser(string username);
        Task DeleteUser(string username);
        Task UpdateUser(User user);
    }
}
