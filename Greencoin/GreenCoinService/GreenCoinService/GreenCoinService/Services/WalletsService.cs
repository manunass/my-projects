using GreenCoinService.DataContracts;
using GreenCoinService.Exceptions;
using GreenCoinService.Services.Interfaces;
using GreenCoinService.Stores.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenCoinService.Services
{
    public class WalletsService : IWalletsService
    {
        private readonly ITransactionsStore _transactionsStore;
        private readonly IWalletsStore _walletsStore;
        private readonly IMunicipalitiesStore _municipalitiesStore;

        public WalletsService(ITransactionsStore transactionsStore, IWalletsStore walletStore, IMunicipalitiesStore municipalitiesStore)
        {
            _transactionsStore = transactionsStore;
            _walletsStore = walletStore;
            _municipalitiesStore = municipalitiesStore;
        }
        public async Task<Transaction> GetTransaction(string transactionId)
        {
            var transaction = await _transactionsStore.GetTransaction(transactionId);
            return transaction;
        }

        public async Task<List<Transaction>> GetTransactions(string walletId)
        {
            var transactions = await _transactionsStore.GetTransactions(walletId);
            return transactions;
        }

        public async Task<Wallet> GetWallet(string walletId)
        {
            var wallet = await _walletsStore.GetWallet(walletId);
            return wallet;
        }
        public async Task<Wallet> GetWalletByOwner(string ownerId)
        {
            var wallet = await _walletsStore.GetWalletByOwner(ownerId);
            return wallet;
        }


        public async Task UpdateWallet(string municipalityId, string walletId, int netAmount, string description, string bagScanId = null)
        {
            var municipality = await _municipalitiesStore.GetMunicipality(municipalityId);
            var wallet = await _walletsStore.GetWallet(walletId);
            var transaction = CreateTransaction(wallet, netAmount, description, bagScanId);
            municipality.CoinsInCirculation += netAmount;
            UpdateWallet(wallet, netAmount);
            await _walletsStore.UpdateWallet(wallet);
            await _transactionsStore.AddTransaction(transaction);
            await _municipalitiesStore.UpdateMunicipality(municipality);
        }

        public async Task Pay(string fromWalletId, string toWalletId, int amount, string description)
        {
            var fromWallet = await _walletsStore.GetWallet(fromWalletId);
            var toWallet = await _walletsStore.GetWallet(toWalletId);
            EnsureValidTransactionOrThrow(fromWallet, amount);
            var transactions = CreateTransactions(fromWallet, toWallet, amount, description);
            fromWallet.Balance -= amount;
            toWallet.Balance += amount;
            await _walletsStore.UpdateWallet(fromWallet);
            await _walletsStore.UpdateWallet(toWallet);
            await _transactionsStore.AddTransaction(transactions.Item1);
            await _transactionsStore.AddTransaction(transactions.Item2);
        }

        private void EnsureValidTransactionOrThrow(Wallet fromWallet, int amount)
        {
            if(fromWallet.Balance < amount)
            {
                throw new BadRequestException("Invalid transaction. You have insufficient funds.");
            }
            if(amount <= 0)
            {
                throw new BadRequestException("Invalid transaction amount. The amount cannot be negative or zero.");
            }
        }
        
        private Tuple<Transaction, Transaction> CreateTransactions(Wallet fromWallet, Wallet toWallet, int amount, string description)
        {
            var transferId = CreateGUID();
            var unixTime = UnixTimeSeconds();
            var transaction1 = new Transaction
            {
                WalletId = fromWallet.Id,
                TransferId = transferId,
                UnixTime = unixTime,
                Description = description,
                PreAmount = fromWallet.Balance,
                Amount = -amount,
                PostAmount = fromWallet.Balance - amount
            };
            var transaction2 = new Transaction
            {
                WalletId = toWallet.Id,
                TransferId = transferId,
                UnixTime = unixTime,
                Description = description,
                PreAmount = toWallet.Balance,
                Amount = amount,
                PostAmount = toWallet.Balance + amount
            };
            return new Tuple<Transaction, Transaction>(transaction1, transaction2);
        }

        private Transaction CreateTransaction(Wallet wallet, int netAmount, string description, string bagScanId = null)
        {
            return new Transaction
            {
                WalletId = wallet.Id,
                TransferId = CreateGUID(),
                UnixTime = UnixTimeSeconds(),
                Description = description,
                BagScanId = bagScanId,
                PreAmount = wallet.Balance,
                Amount = netAmount,
                PostAmount = wallet.Balance + netAmount
            };
        }

        private static void UpdateWallet(Wallet wallet, int netAmount)
        {
            wallet.Balance += netAmount;
            if(netAmount >= 0)
            {
                wallet.Score += netAmount;
            }
        }

        private string CreateGUID()
        {
            return Guid.NewGuid().ToString();
        }

        private static long UnixTimeSeconds()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }
}
