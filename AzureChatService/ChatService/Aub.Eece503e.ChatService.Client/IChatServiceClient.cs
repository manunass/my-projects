using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Aub.Eece503e.ChatService.Datacontracts;

namespace Aub.Eece503e.ChatService.Client
{
    public interface IChatServiceClient
    {
        Task AddUser(User user);
        Task<User> GetUser(string username);
        Task UpdateUser(User user);
        Task DeleteUser(string username);
        Task<UploadImageResponse> UploadImage(Stream stream);
        Task<DownloadImageResponse> DownloadImage(string imageId);
        Task DeleteImage(string imageId);
        Task AddMessage(string conversationId, Message message);
        Task <GetMessagesResponse> GetMessages(string conversationId, int limit, long lastSeenMessageTime);
        Task<GetMessagesResponse> GetMessagesByUri(string uri);
        Task DeleteMessage(string conversationId, string messageId);
        Task<GetConversationsResponse> GetConversations(string username, int limit, long LastSeenConversationTime);
        Task<GetConversationsResponse> GetConversationsByUri(string uri);
        Task<AddConversationResponse> AddConversation(Message firstMessage, List<string> participants);
        Task DeleteConversation(string conversationId);
    }
}
