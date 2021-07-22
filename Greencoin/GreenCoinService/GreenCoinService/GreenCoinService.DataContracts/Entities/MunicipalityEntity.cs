using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GreenCoinService.DataContracts.Entities
{
    public class MunicipalityEntity
    {
        public string Id { get; set; }
        public string MunicipalityName { get; set; }
        public string MohafazaName { get; set; }
        public string QazaName { get; set; }
        public AddressEntity Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public float LbpCoinRatio { get; set; }
        public int CoinsCashoutTreshold { get; set; }
        public int CoinsInCirculation { get; set; }
        public List<UserEntity> Users { get; set; }
        public List<BusinessEntity> Businesses { get; set; }
        public List<EmployeeEntity> Employees { get; set; }
        public List<BatchEntity> Batches { get; set; }
    }
}
