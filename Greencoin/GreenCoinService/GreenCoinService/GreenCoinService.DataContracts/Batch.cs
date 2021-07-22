using System;
using System.Collections.Generic;
using System.Text;

namespace GreenCoinService.DataContracts
{
    public class Batch
    {
        public string Id { get; set; }
        public string MunicipalityId { get; set; }
        public string RecyclableId { get; set; }
        public Recyclable Recyclable { get; set; }
        public int BatchNumber { get; set; }
        public float Revenue { get; set; }
        public bool Current { get; set; }
    }
}
