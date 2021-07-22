using System;
namespace GreenCoinService.DataContracts
{
    public class User : IPlatformUser
    {
        public string Id { get; set; }
        public string FirebaseUid { get; set; }
        public string MunicipalityId { get; set; }
        public Address Address { get; set; }
        public Wallet Wallet { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsVerified { get; set; }
    }
}
