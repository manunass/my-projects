using System.Collections.Generic;

namespace Aub.Eece503e.ChatService.Datacontracts
{
    public class Conversation
    {
        public string Id { get; set; }
        public List<string> Participants { get; set; }
        public long LastModifiedUnixTime { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Conversation conversation &&
                Id == conversation.Id &&
                Participants[0] == conversation.Participants[0] &&
                Participants[1] == conversation.Participants[1];
        }

        public override int GetHashCode()
        {
            var hashCode = -256925990;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Participants[0]);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Participants[1]);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<long>.Default.GetHashCode(LastModifiedUnixTime);

            return hashCode;
        }
    }
}
