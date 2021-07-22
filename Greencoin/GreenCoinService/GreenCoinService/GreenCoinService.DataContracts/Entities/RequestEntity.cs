using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GreenCoinService.DataContracts.Entities
{
    public class RequestEntity
    {
        public string Id { get; set; }
        public string BusinessId { get; set; }
        public BusinessEntity Business { get; set; }
        public string UserId { get; set; }
        public UserEntity User {get;set;}
        public long UnixTimeRequested { get; set; }
        public long UnixTimeApproved { get; set; }
        public long UnixTimeCompleted { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }
}
