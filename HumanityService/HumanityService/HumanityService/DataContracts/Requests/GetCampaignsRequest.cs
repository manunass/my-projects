using System;
using System.Collections.Generic;
using System.Text;

namespace HumanityService.DataContracts.Requests
{
    public class GetCampaignsRequest
    {
        public string NgoName { get; set; }
        public string Username { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
    }
}
