using System;
using System.Collections.Generic;
using System.Text;

namespace GreenCoinService.DataContracts
{
    public class Address
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string AreaOrStreet { get; set; }
        public string Building { get; set; }
        public string Flat { get; set; }
    }
}
