using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GreenCoinService.DataContracts.Entities
{
    public class RecyclableEntity
    {
        public string Id { get; set; }
        public string Material { get; set; }
        public float LbpPerKg { get; set; }
        public List<CodeEntity> Codes { get; set; }
        public List<BatchEntity> Batches { get; set; }
    }
}
