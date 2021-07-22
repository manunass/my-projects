using HumanityService.DataContracts.CompositeDesignPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanityService.DataContracts.Results
{
    public class MatchDeliveryDemandResult
    {
        public DeliveryDemand DeliveryDemand { get; set; }
        public double ETA { get; set; }
    }
}
