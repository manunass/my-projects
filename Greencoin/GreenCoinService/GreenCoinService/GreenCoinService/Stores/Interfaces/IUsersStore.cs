using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GreenCoinService.DataContracts;
using GreenCoinService.DataContracts.Entities;
using GreenCoinService.DataContracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace GreenCoinService.Stores.Interfaces
{
    public interface IUsersStore
    {
        Task <User> AddUser(User user);
        Task UpdateUser(User user);
        Task DeleteUser(string userId);
        Task<User> GetUser(string userId);
        Task<string> GetUserId(string wasteBagCode);
        Task<List<User>> GetUsers(string municipalityId);
        Task<List<User>> SearchUsers(string municipalityId, string firstName, string lastName);

    }
}
