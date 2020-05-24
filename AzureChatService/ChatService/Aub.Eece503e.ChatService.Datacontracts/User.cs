using System;
using System.Collections.Generic;

namespace Aub.Eece503e.ChatService.Datacontracts
{
    public class User
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePictureId { get; set; }


        public override bool Equals(object obj)
        {
            return obj is User user &&
                   Username == user.Username &&
                   FirstName == user.FirstName &&
                   LastName == user.LastName &&
                   ProfilePictureId == user.ProfilePictureId;
        }

        public override int GetHashCode()
        {
            var hashCode = -256925990;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Username);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FirstName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(LastName);
            return hashCode;
        }
    }
}
