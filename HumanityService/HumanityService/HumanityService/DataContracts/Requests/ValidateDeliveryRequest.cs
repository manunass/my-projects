using System;
using System.Collections.Generic;
using System.Text;

namespace HumanityService.DataContracts.Requests
{
    public class ValidateDeliveryRequest
    {
        public string ValidationType { get; set; } //Pickup or Destination
        public string ContributionId { get; set; } //for donor
        public string CampaignId { get; set; } //for ngo
        public string DeliveryCode { get; set; }
    }
}
