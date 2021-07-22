using System;
using System.Collections.Generic;
using System.Text;

namespace GreenCoinService.DataContracts.Requests
{
    public class UpdateWalletsRequest
    {
        public List<UpdateWallet> UpdateWallets { get; set; }
        public string Description { get; set; }
    }

    public class UpdateWallet
    {
        public string WalletId { get; set; }
        public int NetAmount { get; set; }
    }
}
