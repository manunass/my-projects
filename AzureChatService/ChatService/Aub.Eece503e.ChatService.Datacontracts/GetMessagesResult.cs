using System.Collections.Generic;

namespace Aub.Eece503e.ChatService.Datacontracts
{
    public class GetMessagesResult
    {
        public List<Message> Messages { get; set; }
        public string ContinuationToken { get; set; }
    }
}