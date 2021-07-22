using System;
using System.Collections.Generic;
using System.Text;

namespace HumanityService.DataContracts.CompositeDesignPattern
{
    public class UserInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Location Location { get; set; }
    }
}
