using System;
using System.Collections.Generic;
using System.Text;

namespace GreenCoinService.DataContracts.Requests
{
    public class UpdateBusinessRequest
    {
        public Address Address { get; set; }
        public string Name { get; set; }
        public string OwnerFirstName { get; set; }
        public string OwnerLastName { get; set; }
        public string Category { get; set; }
        public string About { get; set; }
    }
}
