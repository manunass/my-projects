using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GreenCoinService.DataContracts.Entities
{
    public class BatchEntity
    {
        public string Id { get; set; }
        public string MunicipalityId { get; set; }
        public MunicipalityEntity Municipality { get; set; }
        public string RecyclableId { get; set; }
        public RecyclableEntity Recyclable { get; set; }
        public int BatchNumber { get; set; }
        public float Revenue { get; set; }
        public bool Current { get; set; }
        public List<BagScanEntity> BagScans { get; set; }
    }
}
