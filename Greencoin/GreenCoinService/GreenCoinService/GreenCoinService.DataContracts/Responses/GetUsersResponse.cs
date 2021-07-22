using System;
using System.Collections.Generic;
using GreenCoinService.DataContracts.Entities;

namespace GreenCoinService.DataContracts.Responses
{
    public class GetUsersResponse
    {
        public List<User> Users { get; set; }
    }
}
