using System;
using System.Collections.Generic;
using System.Text;

namespace GreenCoinService.DataContracts
{
    public class Business : IPlatformUser
    {
        public string Id { get; set; }
        public string FirebaseUid { get; set; }
        public string MunicipalityId { get; set; }
        public Address Address { get; set; }
        public Wallet Wallet { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string OwnerFirstName { get; set; }
        public string OwnerLastName { get; set; }
        public string Category { get; set; }
        public string About { get; set; }
        public List<string> ImagesUris { get; set; }
        public bool IsVerified { get; set; }
    }
}
