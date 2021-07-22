using System;
using System.Collections.Generic;
using System.Text;

namespace GreenCoinService.DataContracts.Responses
{
    public class GetBagScansResponse
    {
        public List<BagScan> BagScans { get; set; }
    }
}
