using HumanityService.DataContracts.CompositeDesignPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace HumanityService.DataContracts.Results
{
    public class GetProcessesResult
    {
        public List<Process> Processes { get; set; }
    }
}
