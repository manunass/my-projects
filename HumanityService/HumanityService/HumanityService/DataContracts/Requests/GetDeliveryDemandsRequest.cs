using System;
using System.Collections.Generic;
using System.Text;

namespace HumanityService.DataContracts.Requests
{
    public class GetDeliveryDemandsRequest
    {
        public string ProcessId { get; set; }
        public string CampaignName { get; set; }
        public string PickupUsername { get; set; }
        public string DestinationUsername { get; set; }
        public string Status { get; set; }
    }
}
