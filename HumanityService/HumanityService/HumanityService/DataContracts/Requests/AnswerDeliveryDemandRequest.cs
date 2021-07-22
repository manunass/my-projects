using HumanityService.DataContracts.CompositeDesignPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace HumanityService.DataContracts.Requests
{
    public class AnswerDeliveryDemandRequest
    {
        public string Username { get; set; }
        public long TimeWindowStart { get; set; }
        public long TimeWindowEnd { get; set; }
        public string OtherInfo { get; set; }
    }
}
