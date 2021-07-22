using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GreenCoinService.DataContracts;
using GreenCoinService.DataContracts.Responses;

namespace GreenCoinService.Services.Interfaces
{
    public interface IUsersService
    {
        Task<User> GetUser(string userId);
        Task<List<User>> GetUsers(string municipalityId);
        Task<string> GetUserId(string wasteBagCode);
        Task<User> AddUser(User user, List<Recyclable> recyclables);
        Task DeleteUser(string userId);
        Task UpdateUser(User user);
        Task<List<User>> SearchUsers(string municipalityId, string firstName, string lastName);
    }
}
