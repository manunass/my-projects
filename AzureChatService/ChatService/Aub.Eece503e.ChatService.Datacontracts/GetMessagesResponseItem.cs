using System;
using System.Collections.Generic;
using System.Text;

namespace Aub.Eece503e.ChatService.Datacontracts
{
    public class GetMessagesResponseItem
    {
        public string Text { get; set; }
        public string SenderUsername { get; set; }
        public long UnixTime { get; set; }

        public override bool Equals(object obj)
        {
            return obj is GetMessagesResponseItem message &&
                message.Text == Text &&
                message.SenderUsername == SenderUsername;
        }

        public override int GetHashCode()
        {
            var hashCode = -256925990;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Text);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SenderUsername);
            return hashCode;
        }
    }
}
