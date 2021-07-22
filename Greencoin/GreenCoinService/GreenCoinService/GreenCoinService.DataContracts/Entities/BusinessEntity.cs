using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GreenCoinService.DataContracts.Entities
{
    public class BusinessEntity
    {
        public string Id { get; set; }
        public string FirebaseUid { get; set; }
        public string MunicipalityId { get; set; }
        public MunicipalityEntity Municipality { get; set; }
        public AddressEntity Address { get; set; }
        public WalletEntity Wallet { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string OwnerFirstName { get; set; }
        public string OwnerLastName { get; set; }
        public string Category { get; set; }
        public string About { get; set; }
        public bool IsVerified { get; set; }
        public List<RequestEntity> Requests { get; set; }
        public List<CodeEntity> Codes { get; set; }
    }
}
