using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GreenCoinService.DataContracts.Entities
{
    public class EmployeeEntity
    {
        public string Id { get; set; }
        public string FirebaseUid { get; set; }
        public string MunicipalityId { get; set; }
        public MunicipalityEntity Municipality { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }
        public List<BagScanEntity> BagScans { get; set; }
    }
}
