using System;
using System.Collections.Generic;
using System.Text;

namespace GreenCoinService.DataContracts
{
    public class BagScan
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string EmployeeId { get; set; }
        public string BatchId { get; set; }
        public double Weight { get; set; }
        public BagScanQuality Quality { get; set; }
        public bool Processed { get; set; }
        public long UnixTimeScanned { get; set; }
        public long UnixTimeProcessed { get; set; }
    }

    public enum BagScanQuality
    {
        VeryBad,
        Bad,
        Good,
        VeryGood,
        Excellent
    }
}
