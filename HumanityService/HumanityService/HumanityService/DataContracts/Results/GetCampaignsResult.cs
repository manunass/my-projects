using HumanityService.DataContracts.CompositeDesignPattern;
using System;
using System.Collections.Generic;

namespace HumanityService.DataContracts.Results
{
    public class GetCampaignsResult
    {
        public List<Campaign> Campaigns { get; set; }
    }
}
