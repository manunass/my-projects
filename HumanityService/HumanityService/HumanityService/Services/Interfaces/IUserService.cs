using HumanityService.DataContracts.CompositeDesignPattern;
using HumanityService.DataContracts.Requests;
using System.Threading.Tasks;

namespace HumanityService.Services.Interfaces
{
    public interface IUserService
    {
        Task AddUser(User user);
        Task<User> GetUser(string username);
        Task DeleteUser(string username);
        Task UpdateUser(User user);
        Task AddNgo(Ngo ngo);
        Task<Ngo> GetNgo(string ngo);
        Task DeleteNgo(string ngo);
        Task UpdateNgo(Ngo ngo);
    }
}
