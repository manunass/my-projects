using System;
using System.Collections.Generic;
using System.Text;

namespace GreenCoinService.DataContracts
{
    public class Wallet
    {
        public string Id { get; set; }
        public int Balance { get; set; }
        public int Score { get; set; }
    }
}
