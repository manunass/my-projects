using System;
using System.Collections.Generic;

namespace Aub.Eece503e.ChatService.Datacontracts
{
    public class AddConversationRequestBody
    {
        
        public List<string> Participants { get; set; }
        public Message FirstMessage { get; set; }
        
    }
}
