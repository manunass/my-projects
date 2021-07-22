using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GreenCoinService.DataContracts.Entities
{
    public class BagScanEntity
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public UserEntity User { get; set; }
        public string EmployeeId { get; set; }
        public EmployeeEntity Employee { get; set; }
        public string BatchId { get; set; }
        public BatchEntity Batch { get; set; }
        public double Weight { get; set; }
        public string Quality { get; set; }
        public bool Processed { get; set; }
        public long UnixTimeScanned { get; set; }
        public long UnixTimeProcessed { get; set; }
        public TransactionEntity Transaction { get; set; }
    }
}
