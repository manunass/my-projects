using GreenCoinService.DataContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenCoinService.Stores.Interfaces
{
    public interface ITransactionsStore
    {
        Task<Transaction> AddTransaction(Transaction transaction);
        Task<Transaction> GetTransaction(string transactionId);
        Task<List<Transaction>> GetTransactions(string walletId);
        Task UpdateTransaction(Transaction transaction);
        Task DeleteTransaction(string transactionId);
    }
}
