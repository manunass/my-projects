using System;
using GreenCoinService.Services.Interfaces;
using System.Threading.Tasks;
using GreenCoinService.DataContracts;
using GreenCoinService.Stores.Interfaces;
using AutoMapper;
using System.Collections.Generic;

namespace GreenCoinService.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersStore _userStore;
        private readonly ICodesService _codesService;

        public UsersService(IUsersStore userStore, ICodesService codesService)
        {
            _userStore = userStore;
            _codesService = codesService;
        }

        public async Task<User> AddUser(User user, List<Recyclable> recyclables)
        {
            InitializeUser(user);
            var result = await _userStore.AddUser(user);
            await _codesService.CreateWasteCodes(user.Id, recyclables);
            return result;
        }

        public async Task DeleteUser(string UserId)
        {
            await _userStore.DeleteUser(UserId);
        }

        public async Task<User> GetUser(string userId)
        {
            var user = await _userStore.GetUser(userId);
            return user;
        }

        public async Task<string> GetUserId(string wasteBagCode)
        {
            var id = await _userStore.GetUserId(wasteBagCode);
            return id;
        }

        public async Task<List<User>> GetUsers(string municipalityId)
        {
            var users  =await  _userStore.GetUsers(municipalityId);
            return users; 
        }

        public async Task UpdateUser(User user)
        {
            await _userStore.UpdateUser(user);
        }

        public async Task<List<User>> SearchUsers(string municipalityId, string firstName, string lastName)
        {
            var users = await _userStore.SearchUsers(municipalityId, firstName, lastName);
            return users;
        }

        private static void InitializeUser(User user)
        {
            user.Wallet = new Wallet
            {
                Balance = 0,
                Score = 0
            };
            user.IsVerified = false;
        }
    }
}
