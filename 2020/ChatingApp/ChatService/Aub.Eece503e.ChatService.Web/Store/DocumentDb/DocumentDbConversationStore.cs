using Microsoft.Extensions.Options;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using Aub.Eece503e.ChatService.Datacontracts;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Aub.Eece503e.ChatService.Web.Store.Exceptions;
using System.Linq;
using Microsoft.Azure.Documents.Linq;
using System.Collections.Generic;
using System.Net;

namespace Aub.Eece503e.ChatService.Web.Store.DocumentDb
{
    public class DocumentDbConversationStore : IConversationStore
    {
        private readonly IDocumentClient _documentClient;
        private readonly IOptions<DocumentDbSettings> _options;

        private Uri DocumentCollectionUri => UriFactory.CreateDocumentCollectionUri(_options.Value.DatabaseName, _options.Value.CollectionName);

        public DocumentDbConversationStore(IDocumentClient documentClient, IOptions<DocumentDbSettings> options)
        {
            _documentClient = documentClient;
            _options = options;
        }

        public async Task AddMessage(string conversationId, Message message) 
        {
            if(!ValidMessage(conversationId, message))
            {
                throw new StorageErrorException($"Failed to store message {message.Id} to conversation {conversationId}", (int)HttpStatusCode.BadRequest);
            }
            try
            {
                var entity = ToMessageEntity(conversationId, message);
                await _documentClient.CreateDocumentAsync(DocumentCollectionUri, entity);
            }
            catch (DocumentClientException e) 
            {
                if ((int)e.StatusCode != 409)
                {
                    throw new StorageErrorException($"Failed to store message {message.Id} to conversation {conversationId}", e, (int)e.StatusCode);
                }
                
            }
        }

        public async Task<GetMessagesResult> GetMessages(string conversationId, string continuationToken, int limit, long lastSeenMessageTime)
        {
            try
            {
                var feedOptions = new FeedOptions
                {
                    MaxItemCount = limit,
                    EnableCrossPartitionQuery = false,
                    RequestContinuation = continuationToken,
                    PartitionKey = new PartitionKey("m_"+conversationId)
                };

                IQueryable<DocumentDbMessageEntity> query = _documentClient.CreateDocumentQuery<DocumentDbMessageEntity>(DocumentCollectionUri, feedOptions)
                    .OrderByDescending(entity => entity.UnixTime);
                query = query.Where(entity => entity.UnixTime > lastSeenMessageTime);
                FeedResponse<DocumentDbMessageEntity> feedResponse = await query.AsDocumentQuery().ExecuteNextAsync<DocumentDbMessageEntity>();
                return new GetMessagesResult
                {
                    ContinuationToken =feedResponse.ResponseContinuation,
                    Messages = feedResponse.Select(ToMessage).ToList()   
                };
            }

            catch (DocumentClientException e)
            {
                throw new StorageErrorException($"Failed to retrieve conversation {conversationId}", e, (int)e.StatusCode);
            }
        }

        public async Task DeleteMessage(string conversationId, string messageId)
        {
            try
            {
                await _documentClient.DeleteDocumentAsync(CreateDocumentUri(messageId), new RequestOptions { PartitionKey = new PartitionKey("m_" + conversationId) });
            }
            catch (DocumentClientException e)
            {
                throw new StorageErrorException($"failed to delete message {messageId} from conversation {conversationId}", e, (int)e.StatusCode);
            }
        }


        public async Task<AddConversationResult> AddConversation(string conversationId, List<string> participants, long unixTime)
        {
            if (!ValidConversation(participants))
            {
                throw new StorageErrorException($"Failed to add conversation with participants {participants[0]} and {participants[1]}", (int)HttpStatusCode.BadRequest);
            }
            try
            {
                var senderEntity = ToConversationEntity(conversationId, participants[0], participants, unixTime);
                var task1 = _documentClient.CreateDocumentAsync(DocumentCollectionUri, senderEntity);
                var recipientEntity = ToConversationEntity(conversationId, participants[1], participants, unixTime);
                var task2 = _documentClient.CreateDocumentAsync(DocumentCollectionUri, recipientEntity);
                await Task.WhenAll(task1, task2);

                return new AddConversationResult
                {
                    Id = conversationId,
                    CreatedUnixTime = unixTime
                };
            }
            catch (DocumentClientException e)
            {
                if ((int)e.StatusCode != 409)
                {
                    throw new StorageErrorException($"Failed to store create conversation {conversationId}", e, (int)e.StatusCode);
                }
                else
                {
                    return new AddConversationResult
                    {
                        Id = conversationId,
                        CreatedUnixTime = unixTime
                    };
                }
            }
        }

        public async Task<AddConversationResult> AddConversationForOneUserTesting(string conversationId, List<string> participants, long unixTime)
        {
            if (!ValidConversation(participants))
            {
                throw new StorageErrorException($"Failed to add conversation with participants {participants[0]} and {participants[0]}", (int)HttpStatusCode.BadRequest); //should I add something here
            }
            try
            {
                var senderEntity = ToConversationEntity(conversationId, participants[0], participants, unixTime);
                await _documentClient.CreateDocumentAsync(DocumentCollectionUri, senderEntity);

                return new AddConversationResult
                {
                    Id = conversationId,
                    CreatedUnixTime = unixTime
                };
            }
            catch (DocumentClientException e)
            {
                if ((int)e.StatusCode != 409)
                {
                    throw new StorageErrorException($"Failed to store create conversation {conversationId}", e, (int)e.StatusCode);
                }
                else
                {
                    return new AddConversationResult
                    {
                        Id = conversationId,
                        CreatedUnixTime = unixTime
                    };
                }
            }
        }

        public async Task<GetConversationsResult> GetConversations(string username, string continuationToken, int limit, long lastSeenConversationTime)
        {
            try
            {
                var feedOptions = new FeedOptions
                {
                    MaxItemCount = limit,
                    EnableCrossPartitionQuery = false,
                    RequestContinuation = continuationToken,
                    PartitionKey = new PartitionKey("c_" + username)
                };

                IQueryable<DocumentDbConversationEntity> query = _documentClient.CreateDocumentQuery<DocumentDbConversationEntity>(DocumentCollectionUri, feedOptions)
                    .OrderByDescending(entity => entity.LastModifiedUnixTime);
                query = query.Where(entity => entity.LastModifiedUnixTime > lastSeenConversationTime);
                FeedResponse<DocumentDbConversationEntity> feedResponse = await query.AsDocumentQuery().ExecuteNextAsync<DocumentDbConversationEntity>();
                return new GetConversationsResult
                {
                    ContinuationToken = feedResponse.ResponseContinuation,
                    Conversations = feedResponse.Select(ToConversation).ToList()
                };
            }

            catch (DocumentClientException e)
            {
                throw new StorageErrorException($"Failed to retrieve conversations of user {username}", e, (int)e.StatusCode);
            }
        }

        public async Task UpdateConversation(string conversationId, Message message)
        {
            try
            {
                var participants = GetParticipants(conversationId);

                await _documentClient.UpsertDocumentAsync(DocumentCollectionUri,ToConversationEntity(conversationId, participants[0], participants, message.UnixTime), 
                    new RequestOptions { PartitionKey = new PartitionKey("c_" + participants[0])});

                await _documentClient.UpsertDocumentAsync(DocumentCollectionUri, ToConversationEntity(conversationId, participants[1], participants, message.UnixTime),
                    new RequestOptions { PartitionKey = new PartitionKey("c_" + participants[1]) });
            }
            catch (DocumentClientException e) 
            {
                throw new StorageErrorException($"Failed to upsert conversation {conversationId}", e, (int)e.StatusCode);
            }
        }
        public async Task DeleteConversation(string conversationId)
        {
            try
            {
                var participants = GetParticipants(conversationId);
                await _documentClient.DeleteDocumentAsync(CreateDocumentUri(conversationId), new RequestOptions { PartitionKey = new PartitionKey("c_" + participants[0]) });
                await _documentClient.DeleteDocumentAsync(CreateDocumentUri(conversationId), new RequestOptions { PartitionKey = new PartitionKey("c_" + participants[1]) });
            }
            catch (DocumentClientException e)
            {
                throw new StorageErrorException($"failed to delete conversation {conversationId}", e, (int)e.StatusCode);
            }
        }


        private static bool ValidMessage(string conversationId, Message message)
        {
            List<string> badRequests = new List<string> { "", " ", null };
            if (badRequests.Contains(conversationId)
                || badRequests.Contains(message.Text)
                || badRequests.Contains(message.Id)
                || badRequests.Contains(message.SenderUsername))
            {
                return false;
            }
            else return true;
        }

        private static bool ValidConversation(List<string> participants)
        {
            List<string> badRequests = new List<string> { "", " ", null };
            if (participants.Count() != 2
                || badRequests.Contains(participants[0])
                || badRequests.Contains(participants[1]))
            {
                return false;
            }
            else return true;
        }

        private static List<string> GetParticipants(string conversationId)
        {
            string separator = "_";
            int count = 2;
            String[] split = conversationId.Split(separator, count);
            List<string> participants = new List<string>();
            participants.Add(split[0]);
            participants.Add(split[1]);
            return participants;
        }

        private static Message ToMessage(DocumentDbMessageEntity entity)
        {
            return new Message
            {
                Id = entity.Id,
                Text = entity.Text,
                SenderUsername = entity.SenderUsername,
                UnixTime = entity.UnixTime
            };
        }

        private static Conversation ToConversation(DocumentDbConversationEntity entity)
        {
            return new Conversation
            {
                Id = entity.Id,
                Participants = entity.Participants,
                LastModifiedUnixTime = entity.LastModifiedUnixTime
            };
        }

        private static DocumentDbConversationEntity ToConversationEntity(string conversationId, string participant, List<string> participants, long LastModifiedTime)
        {
            return new DocumentDbConversationEntity
            {
                PartitionKey = "c_" + participant,
                Id = conversationId,
                Participants = participants,
                LastModifiedUnixTime = LastModifiedTime
            };
        }
        private static DocumentDbMessageEntity ToMessageEntity(string conversationId, Message message)
        {
            return new DocumentDbMessageEntity
            {
                Id = message.Id,
                PartitionKey = "m_" + conversationId,
                Text = message.Text,
                SenderUsername = message.SenderUsername,
                UnixTime = message.UnixTime
            };
        }


        class DocumentDbConversationEntity
        {
            public string PartitionKey { get; set; }
            [JsonProperty("id")]
            public string Id { get; set; }
            public List<string> Participants { get; set; }
            public long LastModifiedUnixTime { get; set; }
        }

        class DocumentDbMessageEntity
        {
            public string PartitionKey { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }
            public string Text { get; set; }
            public string SenderUsername { get; set; }
            public long UnixTime { get; set; }
        }

        private Uri CreateDocumentUri(string documentId)
        {
            return UriFactory.CreateDocumentUri(_options.Value.DatabaseName, _options.Value.CollectionName, documentId);
        }
    }
}
