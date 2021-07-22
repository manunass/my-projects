using System;
using System.Collections.Generic;
using System.Text;

namespace GreenCoinService.DataContracts
{
    public class MunicipalityInfo
    {
        public string Id { get; set; }
        public string MunicipalityName { get; set; }
        public string MohafazaName { get; set; }
        public string QazaName { get; set; }
        public Address Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
