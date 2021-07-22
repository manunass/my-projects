using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GreenCoinService.DataContracts.Entities
{
    public class TransactionEntity
    {
        public string Id { get; set; }
        public string WalletId { get; set; }
        public WalletEntity Wallet { get; set; }
        public string TransferId { get; set; }
        public string BagScanId { get; set; }
        public BagScanEntity BagScan { get; set; }
        public long UnixTime { get; set; }
        public string Description { get; set; }
        public int PreAmount { get; set; }
        public int Amount { get; set; }
        public int PostAmount { get; set; }
    }
}
