using System;
using System.Collections.Generic;
using System.Text;

namespace GreenCoinService.DataContracts.Responses
{
    public class GetTransactionsResponse
    {
        public List<Transaction> Transactions { get; set; }
    }
}
