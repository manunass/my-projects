using System;
using System.Collections.Generic;
using System.Text;

namespace GreenCoinService.DataContracts.Requests
{
    public class CreateBagScanRequest
    {
        public string UserId { get; set; }
        public string EmployeeId { get; set; }
        public double Weight { get; set; }
        public BagScanQuality Quality { get; set; }
    }
}
