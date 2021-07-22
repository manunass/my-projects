﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GreenCoinService.DataContracts
{
    public class Transaction
    {
        public string Id { get; set; }
        public string WalletId { get; set; }
        public string TransferId { get; set; }
        public string BagScanId { get; set; }
        public long UnixTime { get; set; }
        public string Description { get; set; }
        public int PreAmount { get; set; }
        public int Amount { get; set; }
        public int PostAmount { get; set; }
    }
}
