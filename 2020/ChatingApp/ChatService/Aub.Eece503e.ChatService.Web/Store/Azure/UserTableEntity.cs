using Microsoft.WindowsAzure.Storage.Table;

namespace Aub.Eece503e.ChatService.Web.Store.Azure
{
    public class UserTableEntity: TableEntity
    {
        public UserTableEntity() // default constructor is mandatory
        {
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePictureId { get; set; }
    }
}
