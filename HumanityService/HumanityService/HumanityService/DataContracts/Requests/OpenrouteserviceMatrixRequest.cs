using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanityService.DataContracts.Requests
{
    public class OpenrouteserviceMatrixRequest
    {
        public List<List<double>> locations { get; set; }
    }
}
