using System;
using System.Collections.Generic;
using System.Text;

namespace Aub.Eece503e.ChatService.Datacontracts
{
    public class GetConversationsItem
    {
        public string Id { get; set; }
        public long LastMofiedUnixTime { get; set; }
        public User Recipient { get; set; }

        public override bool Equals(object obj)
        {
            return obj is GetConversationsItem conversation &&
                conversation.Id == Id &&
                conversation.LastMofiedUnixTime == LastMofiedUnixTime &&
                conversation.Recipient.Equals(Recipient);
        }

        public override int GetHashCode()
        {
            var hashCode = -256925990;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<long>.Default.GetHashCode(LastMofiedUnixTime);
            hashCode = hashCode * -1521134295 + Recipient.GetHashCode();
            return hashCode;
        }

    }
}
