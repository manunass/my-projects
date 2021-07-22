using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GreenCoinService.DataContracts.Entities
{
    public class CodeEntity
    {
        public string Id { get; set; }
        public string RecyclableId { get; set; }
        public RecyclableEntity Recyclable { get; set; }
        public string BusinessId { get; set; }
        public BusinessEntity Business { get; set; }
        public string UserId { get; set; }
        public UserEntity User { get; set; }
        public string Type { get; set; }
    }
}
