using GreenCoinService.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenCoinService.Stores.Interfaces
{
    public interface IWalletsStore
    {
        Task<Wallet> AddWallet(Wallet wallet);
        Task<Wallet> GetWallet(string walletId);
        Task<Wallet> GetWalletByOwner(string ownerId);
        Task UpdateWallet(Wallet wallet);
        Task DeleteWallet(string walletId);
    }
}
