using System;
using System.Collections.Generic;

namespace Aub.Eece503e.ChatService.Datacontracts
{
    public class GetConversationsResult


    {        
        public List<Conversation> Conversations { get; set; }
        public List<GetConversationsItem> GetConversationsItems { get; set; }
        public string ContinuationToken {get; set;}
    }
}
