using System;
using System.Collections.Generic;
using System.Text;

namespace GreenCoinService.DataContracts
{
    public class Code
    {
        public string Id { get; set; }
        public string RecyclableId { get; set; }
        public Recyclable Recyclable { get; set; }
        public string BusinessId { get; set; }
        public string UserId { get; set; }
        public CodeType Type { get; set; }
    }

    public enum CodeType
    {
        Business,
        Waste
    }
}
