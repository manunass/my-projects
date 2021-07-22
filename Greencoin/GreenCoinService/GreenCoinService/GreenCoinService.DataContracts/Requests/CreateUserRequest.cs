using System;
namespace GreenCoinService.DataContracts.Requests
{
    public class CreateUserRequest
    {
        public string FirebaseUid { get; set; }
        public string MunicipalityId { get; set; }
        public Address Address { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
