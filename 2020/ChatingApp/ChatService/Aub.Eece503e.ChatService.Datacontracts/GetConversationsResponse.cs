using System;
using System.Collections.Generic;
using System.Text;

namespace Aub.Eece503e.ChatService.Datacontracts
{
    public class GetConversationsResponse
    {
        public List<GetConversationsItem> Conversations { get; set; }
        public string NextUri { get; set; }
    }
}
