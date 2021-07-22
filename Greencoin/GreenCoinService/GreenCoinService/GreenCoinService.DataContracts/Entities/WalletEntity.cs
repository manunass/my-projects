using System;
using System.Collections.Generic;

namespace GreenCoinService.DataContracts.Entities
{
    public class WalletEntity
    {
        public string Id { get; set; }
        public int Balance { get; set; }
        public int Score { get; set; }
        public List<TransactionEntity> Transactions { get; set; }
        public string BusinessId { get; set; }
        public BusinessEntity Business {get;set;}
        public string UserId { get; set; }
        public UserEntity User { get; set; }
    }
}
