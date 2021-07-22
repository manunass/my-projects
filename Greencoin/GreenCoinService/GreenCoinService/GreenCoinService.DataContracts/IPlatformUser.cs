using System;
using System.Collections.Generic;
using System.Text;

namespace GreenCoinService.DataContracts
{
    public interface IPlatformUser
    {
        public string Id { get; set; }
        public string FirebaseUid { get; set; }
        public string MunicipalityId { get; set; }
        public Wallet Wallet { get; set; }
    }
}
