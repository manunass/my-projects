using Aub.Eece503e.ChatService.Datacontracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aub.Eece503e.ChatService.Web.Services
{
    public interface IConversationService
    {
        Task AddMessage(Message message, string conversationId);
        Task<GetMessagesResult> GetMessages(string conversationId, string continuationToken, int limit, long lastSeenMessageTime);
        Task DeleteMessage(string conversationId, string messageId);
        Task<AddConversationResult> AddConversation(Message firstMessage, List<string> participants);
        Task<GetConversationsResult> GetConversations(string username, string continuationToken, int limit, long lastSeenConversationTime);
        Task DeleteConversation(string conversationId);
    }
}
