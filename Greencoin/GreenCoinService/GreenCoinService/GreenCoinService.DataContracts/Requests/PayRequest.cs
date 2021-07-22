using System;
using System.Collections.Generic;
using System.Text;

namespace GreenCoinService.DataContracts.Requests
{
    public class PayRequest
    {
        public string FromWalletId { get; set; }
        public string ToWalletId { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
    }
}
