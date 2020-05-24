using Aub.Eece503e.ChatService.Datacontracts;
using Aub.Eece503e.ChatService.Web.Store;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Aub.Eece503e.ChatService.Web.Services
{
    public class ConversationService : IConversationService 
    {
        private readonly IConversationStore _conversationStore;
        private readonly IProfileStore _profileStore;
        private readonly ILogger<ConversationService> _logger;
        private readonly TelemetryClient _telemetryClient;

        public ConversationService(IConversationStore conversationStore, IProfileStore profileStore, ILogger<ConversationService> logger, TelemetryClient telemetryClient )
        {
            _conversationStore = conversationStore;
            _profileStore = profileStore;
            _logger = logger;
            _telemetryClient = telemetryClient;
        }

        public async Task AddMessage(Message message, string conversationId)
        {
            using (_logger.BeginScope("{conversationId}", conversationId))
            { 
                var stopWatch = Stopwatch.StartNew();
                message.UnixTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                await _conversationStore.AddMessage(conversationId, message);
                await _conversationStore.UpdateConversation(conversationId, message);
                _telemetryClient.TrackMetric("ConversationStore.PostMessage.Time", stopWatch.ElapsedMilliseconds);
                _telemetryClient.TrackEvent("MessagePosted");
            }
        }

        public async Task<GetMessagesResult> GetMessages(string conversationId, string continuationToken, int limit, long lastSeenMessageTime)
        {
            using (_logger.BeginScope("{conversationId}", conversationId))
            {
                var stopWatch = Stopwatch.StartNew();
                GetMessagesResult result = await _conversationStore.GetMessages(conversationId, continuationToken, limit, lastSeenMessageTime);
                _telemetryClient.TrackMetric("ConversationStore.GetMessages.Time", stopWatch.ElapsedMilliseconds);
                return result;
            }
        }

        public async Task DeleteMessage(string conversationId, string messageId)
        {
            using (_logger.BeginScope("{conversationId}", conversationId))
            {
                var stopWatch = Stopwatch.StartNew();
                await _conversationStore.DeleteMessage(conversationId, messageId);
                _telemetryClient.TrackMetric("ConversationStore.DeleteMessage.Time", stopWatch.ElapsedMilliseconds);
                _telemetryClient.TrackEvent("MessageDeleted");
            }
        }

        public async Task<AddConversationResult> AddConversation(Message firstMessage, List<string> participants)
        {
            using (_logger.BeginScope("{SenderId}",firstMessage.SenderUsername))
            {
                var stopWatch = Stopwatch.StartNew();
                firstMessage.UnixTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                string conversationId = participants[0] + "_" + participants[1];
                await _conversationStore.AddMessage(conversationId, firstMessage);
                AddConversationResult result = await _conversationStore.AddConversation(conversationId, participants, firstMessage.UnixTime);
                _telemetryClient.TrackMetric("ConversationStore.AddConversation.Time", stopWatch.ElapsedMilliseconds);
                _telemetryClient.TrackEvent("ConversationAdded");
                return result;
            }
        }


        public async Task<GetConversationsResult> GetConversations(string username, string continuationToken, int limit, long lastSeenConversationTime)
        {
            using (_logger.BeginScope("{username}", username))
            {
                var stopWatch = Stopwatch.StartNew();
                GetConversationsResult result = await _conversationStore.GetConversations(username, continuationToken, limit, lastSeenConversationTime);
                result.GetConversationsItems = await GetConversationsUsers(username, result);
                _telemetryClient.TrackMetric("ConversationStore.GetConversations.Time", stopWatch.ElapsedMilliseconds);
                return result;
            }
        }

        public async Task<List<GetConversationsItem>> GetConversationsUsers(string username, GetConversationsResult result)
        {
            List<GetConversationsItem> items = new List<GetConversationsItem>();
            foreach (var conversation in result.Conversations)
            {
                int user = Convert.ToInt32(conversation.Participants[0] == username); 
                User recipient = await _profileStore.GetUser(conversation.Participants[user]);
                items.Add(
                    new GetConversationsItem
                    {
                        Id = conversation.Id,
                        LastMofiedUnixTime = conversation.LastModifiedUnixTime,
                        Recipient = recipient
                    });
            }
            return items;
        }

        public async Task DeleteConversation(string conversationId)
        {
            using (_logger.BeginScope("{conversationId}", conversationId))
            {
                var stopWatch = Stopwatch.StartNew();
                await _conversationStore.DeleteConversation(conversationId);
                _telemetryClient.TrackMetric("ConversationStore.DeleteConversation.Time", stopWatch.ElapsedMilliseconds);
                _telemetryClient.TrackEvent("ConversationDeleted");
            }
        }
    }
}

