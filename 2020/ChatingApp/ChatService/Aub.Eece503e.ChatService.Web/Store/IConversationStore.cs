using Aub.Eece503e.ChatService.Datacontracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aub.Eece503e.ChatService.Web.Store
{
    public interface IConversationStore
    {
        Task AddMessage(string conversationId, Message message);
        Task<GetMessagesResult> GetMessages(string conversationId, string continuationToken, int limit, long lastSeenMessageTime);
        Task DeleteMessage(string conversationId, string messageId);
        Task<AddConversationResult> AddConversation(string conversationId, List<string> participants, long unixTime);
        Task<AddConversationResult> AddConversationForOneUserTesting(string conversationId, List<string> participants, long unixTime);
        Task<GetConversationsResult> GetConversations(string username, string continuationToken, int limit, long lastSeenConversationTime);
        Task UpdateConversation(string conversationId, Message message);
        Task DeleteConversation(string conversationId);

    }
}
