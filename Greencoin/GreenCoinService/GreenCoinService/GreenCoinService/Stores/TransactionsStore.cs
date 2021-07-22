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
    public class TransactionsStore : ITransactionsStore
    {
        private readonly IDbContextFactory<GreenCoinServiceDbContext> _contextFactory;
        private readonly IMapper _mapper;

        public TransactionsStore(IDbContextFactory<GreenCoinServiceDbContext> contextFactory)
        {
            _contextFactory = contextFactory;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Transaction, TransactionEntity>();
                cfg.CreateMap<TransactionEntity, Transaction>();
            });
            _mapper = configuration.CreateMapper();
        }

        public async Task<Transaction> AddTransaction(Transaction transaction)
        {
            using var context = _contextFactory.CreateDbContext();
            transaction.Id = CreateGUID();
            var transactionEntity = _mapper.Map<TransactionEntity>(transaction);
            context.Transactions.Add(transactionEntity);
            await context.SaveChangesAsync();
            return transaction;
        }

        public async Task DeleteTransaction(string transactionId)
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                var transactionEntity = new TransactionEntity
                {
                    Id = transactionId
                };
                context.Transactions.Remove(transactionEntity);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new StorageErrorException($"Transaction entity with Id {transactionId} was not found", 404);
            }
        }

        public async Task<Transaction> GetTransaction(string transactionId)
        {
            using var context = _contextFactory.CreateDbContext();
            var transactionEntity = await context.Transactions.Where(transaction => transaction.Id == transactionId).FirstOrDefaultAsync();
            if (transactionEntity == null)
            {
                throw new StorageErrorException($"Transaction entity with Id {transactionId} was not found", 404);
            }
            var transaction = _mapper.Map<Transaction>(transactionEntity);
            return transaction;
        }

        public async Task<List<Transaction>> GetTransactions(string walletId)
        {
            using var context = _contextFactory.CreateDbContext();
            var transactionEntities = await context.Transactions.Where(transaction => transaction.WalletId == walletId).ToListAsync();
            var transactions = _mapper.Map<List<Transaction>>(transactionEntities);
            return transactions;
        }

        public async Task UpdateTransaction(Transaction transaction)
        {
            using var context = _contextFactory.CreateDbContext();
            var transactionEntity = await context.Transactions
                .Where(e => e.Id == transaction.Id)
                .FirstOrDefaultAsync();
            if (transactionEntity == null)
            {
                throw new StorageErrorException($"Transaction entity with Id {transaction.Id} was not found", 404);
            }
            _mapper.Map(transaction, transactionEntity);
            await context.SaveChangesAsync();
        }

        private string CreateGUID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
