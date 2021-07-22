using AutoMapper;
using GreenCoinService.DataContracts;
using GreenCoinService.DataContracts.Entities;
using GreenCoinService.Exceptions;
using GreenCoinService.Migrations;
using GreenCoinService.Stores.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenCoinService.Stores
{
    public class WalletsStore : IWalletsStore
    {
        private readonly IDbContextFactory<GreenCoinServiceDbContext> _contextFactory;
        private readonly IMapper _mapper;

        public WalletsStore(IDbContextFactory<GreenCoinServiceDbContext> contextFactory)
        {
            _contextFactory = contextFactory;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Wallet, WalletEntity>();
                cfg.CreateMap<WalletEntity, Wallet>();
            });
            _mapper = configuration.CreateMapper();
        }

        public async Task<Wallet> AddWallet(Wallet wallet)
        {
            using var context = _contextFactory.CreateDbContext();
            wallet.Id = CreateGUID();
            var walletEntity = _mapper.Map<WalletEntity>(wallet);
            context.Wallets.Add(walletEntity);
            await context.SaveChangesAsync();
            return wallet;
        }

        public async Task DeleteWallet(string walletId)
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                var walletEntity = await context.Wallets.Where(e => e.Id == walletId).Include(e => e.Transactions).FirstOrDefaultAsync();
                context.Wallets.Remove(walletEntity);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new StorageErrorException($"Wallet entity with Id {walletId} was not found", 404);
            }
        }

        public async Task<Wallet> GetWallet(string walletId)
        {
            using var context = _contextFactory.CreateDbContext();
            var walletEntity = await context.Wallets.Where(wallet => wallet.Id == walletId).FirstOrDefaultAsync();
            if (walletEntity == null)
            {
                throw new StorageErrorException($"Wallet entity with Id {walletId} was not found", 404);
            }
            var wallet = _mapper.Map<WalletEntity, Wallet>(walletEntity);
            return wallet;
        }

        public async Task<Wallet> GetWalletByOwner(string ownerId)
        {
            using var context = _contextFactory.CreateDbContext();
            var walletEntity = await context.Wallets
                .Where(wallet => wallet.UserId == ownerId || wallet.BusinessId == ownerId)
                .FirstOrDefaultAsync();
            if (walletEntity == null)
            {
                throw new StorageErrorException($"Wallet entity with owner Id {ownerId} was not found", 404);
            }
            var wallet = _mapper.Map<WalletEntity, Wallet>(walletEntity);
            return wallet;
        }

        public async  Task UpdateWallet(Wallet wallet)
        {
            using var context = _contextFactory.CreateDbContext();
            var walletEntity = await context.Wallets
                .Where(e => e.Id == wallet.Id)
                .FirstOrDefaultAsync();
            if (walletEntity == null)
            {
                throw new StorageErrorException($"Wallet entity with Id {wallet.Id} was not found", 404);
            }
            _mapper.Map(wallet, walletEntity);
            await context.SaveChangesAsync();
        }

        private string CreateGUID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
