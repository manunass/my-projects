using System;
using System.Collections.Generic;
using System.Text;

namespace GreenCoinService.DataContracts.Requests
{
    public class CreateMunicipalityRequest
    {
        public string MunicipalityName { get; set; }
        public string MohafazaName { get; set; }
        public string QazaName { get; set; }
        public Address Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public double LbpCoinRatio { get; set; }
        public double CoinsCashoutTreshold { get; set; }
        public int CoinsInCirculation { get; set; }
    }
}
