using HumanityService.DataContracts.CompositeDesignPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanityService.DataContracts.Requests
{
    public class MatchCampaignRequest
    {
        public string Type { get; set; }
        public string Category { get; set; }
        public string TransportationType { get; set; }
        public Location Location { get; set; }
    }
}
