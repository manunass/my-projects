using GreenCoinService.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenCoinService.Services.Interfaces
{
    public interface IWalletsService
    {
        //TODO: SearchTransactions and GetWalletId
        Task<Wallet> GetWallet(string walletId);
        Task<Wallet> GetWalletByOwner(string ownerId);
        Task UpdateWallet(string municipalityId, string walletId, int netAmount, string description, string bagScanId = null);
        Task Pay(string fromWalletId, string toWalletId, int amount, string description);
        Task<Transaction> GetTransaction(string transactionId);
        Task<List<Transaction>> GetTransactions(string walletId);
    }
}
