using System;
using System.Collections.Generic;
using System.Text;

namespace Aub.Eece503e.ChatService.Datacontracts
{
    public class GetMessagesResponse
    {
        public List<GetMessagesResponseItem> Messages { get; set; }
        public string NextUri { get; set; }
    }
}
