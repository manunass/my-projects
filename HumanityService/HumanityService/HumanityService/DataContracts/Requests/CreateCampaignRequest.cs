using System;
using System.Collections.Generic;
using System.Text;

namespace HumanityService.DataContracts.Requests
{
    public class CreateCampaignRequest
    {
        public string Name { get; set; }
        public string NgoUsername { get; set; }
        public string NgoName { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public int Target { get; set; }
    }
}
