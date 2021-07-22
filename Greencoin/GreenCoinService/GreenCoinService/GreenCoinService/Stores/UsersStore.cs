using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GreenCoinService.DataContracts;
using GreenCoinService.DataContracts.Entities;
using GreenCoinService.Exceptions;
using GreenCoinService.Migrations;
using GreenCoinService.Stores.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GreenCoinService.Stores
{
    public class UsersStore : IUsersStore
    {
        private readonly IDbContextFactory<GreenCoinServiceDbContext> _contextFactory;
        private readonly IMapper _mapper;
        public UsersStore(IDbContextFactory<GreenCoinServiceDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserEntity>();
                cfg.CreateMap<UserEntity, User>();
                cfg.CreateMap<Address, AddressEntity>();
                cfg.CreateMap<AddressEntity, Address>();
                cfg.CreateMap<Wallet, WalletEntity>();
                cfg.CreateMap<WalletEntity, Wallet>();
            });
            _mapper = configuration.CreateMapper();
        }
    

        public async Task<User> AddUser(User user)
        {
            using var context = _contextFactory.CreateDbContext();
            var userId = CreateGUID();
            user.Id = userId;
            var userEntity = _mapper.Map<UserEntity>(user);
            InitializeUserEntity(userEntity, userId);
            context.Users.Add(userEntity);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task DeleteUser(string userId)
        {
            try
            { 
                using var context = _contextFactory.CreateDbContext();
                var userEntity = await context.Users.Where(e => e.Id == userId)
                    .Include(e => e.Address)
                    .Include(e => e.Wallet)
                        .ThenInclude(e => e.Transactions)
                    .Include(e => e.Requests)
                    .Include(e => e.Codes)
                    .FirstOrDefaultAsync();
                context.Users.Remove(userEntity);
                await context.SaveChangesAsync();
             }
            catch
            {
                throw new StorageErrorException($" User with Id {userId} was not found", 404);
            }
        }

        public async Task UpdateUser(User user)
        {
            using var context = _contextFactory.CreateDbContext();
            var userEntity = await context.Users
                .Where(e => e.Id == user.Id)
                .Include(e => e.Address)
                .Include(e => e.Wallet)
                .FirstOrDefaultAsync();
            if (userEntity == null)
            {
                throw new StorageErrorException($"User entity with Id {user.Id} was not found", 404);
            }
            _mapper.Map(user, userEntity);
            await context.SaveChangesAsync();
        }

        public async Task<User> GetUser(string userId)
        {
            using var context = _contextFactory.CreateDbContext();
            var userEntity = await context.Users
                .Where(e => e.Id == userId)
                .Include(e => e.Address)
                .Include(e => e.Wallet)
                .FirstOrDefaultAsync();
            if (userEntity == null)
            {
                throw new StorageErrorException($"User entity with Id {userId} was not found", 404);
            }
            var user = _mapper.Map<User>(userEntity);
            return user;
        }

        public async Task<string> GetUserId(string wasteBagCode)
        {
            using var context = _contextFactory.CreateDbContext();
            var codeEntity = await context.Codes
                .Where(e => e.Id == wasteBagCode)
                .FirstOrDefaultAsync();
            if (codeEntity == null)
            {
                throw new StorageErrorException($" no code with value {wasteBagCode} was not found", 404);
            }
            return codeEntity.UserId;
        }

        public async Task<List<User>> GetUsers(string municipalityId)
        {
            using var context = _contextFactory.CreateDbContext();
            var userEntities = await context.Users
                .Where(e => e.MunicipalityId == municipalityId)
                .Include(e => e.Address)
                .Include(e => e.Wallet)
                .ToListAsync();
            var users = _mapper.Map<List<User>>(userEntities);

            return users;
        }

        public async Task<List<User>> SearchUsers(string municipalityId, string firstName, string lastName)
        {
            using var context = _contextFactory.CreateDbContext();
            var userEntities = await context.Users
                .Where(e => e.MunicipalityId == municipalityId)
                .Where(e => firstName == null || e.FirstName == firstName)
                .Where(e => lastName == null || e.LastName == lastName)
                .Include(e => e.Address)
                .ToListAsync();
            var users = _mapper.Map<List<User>>(userEntities);
            return users;
        }

        private static void InitializeUserEntity(UserEntity userEntity, string userId)
        {
            userEntity.Address.Id = CreateGUID();
            userEntity.Address.UserId = userId;
            userEntity.Wallet.Id = CreateGUID();
            userEntity.Wallet.UserId = userId;
        }

        private static string CreateGUID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
