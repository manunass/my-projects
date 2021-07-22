using System;
using System.Collections.Generic;
using System.Text;

namespace GreenCoinService.DataContracts.Requests
{
    public class UpdateMunicipalityRequest
    {
        public Address Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public float LbpCoinRatio { get; set; }
        public int CoinsCashoutTreshold { get; set; }
        public int CoinsInCirculation { get; set; }
    }
}
