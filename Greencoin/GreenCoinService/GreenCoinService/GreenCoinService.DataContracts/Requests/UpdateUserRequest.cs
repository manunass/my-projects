using System;
namespace GreenCoinService.DataContracts.Requests
{
    public class UpdateUserRequest
    {
        public Address Address { get; set; }
        public string Email { get; set; }
    }
}
