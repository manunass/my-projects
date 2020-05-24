using Aub.Eece503e.ChatService.Datacontracts;
using System.Threading.Tasks;

namespace Aub.Eece503e.ChatService.Web.Services
{
    public interface IProfileService
    {
        Task AddUser(User user);
        Task<User> GetUser(string username);
        Task UpdateUser(string username, User updatedUser);
        Task DeleteUser(string username);
    }
}
