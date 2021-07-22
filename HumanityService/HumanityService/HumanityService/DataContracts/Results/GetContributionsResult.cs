using HumanityService.DataContracts.CompositeDesignPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace HumanityService.DataContracts.Results
{
    public class GetContributionsResult
    {
        public List<Contribution> Contributions { get; set; }
    }
}
