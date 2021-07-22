using System;
using System.Collections.Generic;
using System.Text;

namespace GreenCoinService.DataContracts.Requests
{
    public class CreateRecyclableRequest
    {
        public string Material { get; set; }
        public float LbpPerKg { get; set; }
    }
}
