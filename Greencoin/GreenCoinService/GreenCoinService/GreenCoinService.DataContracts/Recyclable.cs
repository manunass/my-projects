using System;
using System.Collections.Generic;
using System.Text;

namespace GreenCoinService.DataContracts
{
    public class Recyclable
    {
        public string Id { get; set; }
        public string Material { get; set; }
        public float LbpPerKg { get; set; }
    }
}
