using System;
using System.Collections.Generic;
using System.Text;

namespace HumanityService.DataContracts.CompositeDesignPattern
{
    public class NgoInfo
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string RegistrationNumber { get; set; }
        public string WebsiteAddress { get; set; }
        public string PhoneNumber { get; set; }
        public Location Location { get; set; }
        public string Description { get; set; }
    }
}
