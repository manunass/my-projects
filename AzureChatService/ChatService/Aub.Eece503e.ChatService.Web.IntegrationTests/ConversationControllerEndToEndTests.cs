using Aub.Eece503e.ChatService.Client;
using Aub.Eece503e.ChatService.Datacontracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;
namespace Aub.Eece503e.ChatService.Web.IntegrationTests
{
    public abstract class ConversationControllerEndToEndTests<TFixture>: IClassFixture<TFixture>, IAsyncLifetime where TFixture : class, IEndToEndTestsFixture
    {
        private readonly IChatServiceClient _chatServiceClient;

        public readonly ConcurrentBag<string> _conversationsToCleanup = new ConcurrentBag<string>();
        public readonly ConcurrentBag<List<string>> _messagesToCleanup = new ConcurrentBag<List<string>>();
        public readonly ConcurrentBag<User> _usersToCleanup = new ConcurrentBag<User>();

        protected ConversationControllerEndToEndTests(TFixture fixture)
        {
            _chatServiceClient = fixture.ChatServiceClient;
        }

        // implementation of IAsyncLifetime interface called by xunit
        public Task InitializeAsync()
        {
            // nothing to initialize
            return Task.CompletedTask;
        }
        // implementation of IAsyncLifetime interface called by xunit
        public async Task DisposeAsync()
        {
            var tasks = new List<Task>();
            foreach (var Item in _messagesToCleanup)
            {
                var task = _chatServiceClient.DeleteMessage(Item[0], Item[1]);
                tasks.Add(task);
            }
            foreach (var Conversation in _conversationsToCleanup)
            {
                var task = _chatServiceClient.DeleteConversation(Conversation);
                tasks.Add(task);
            }
            foreach (var user in _usersToCleanup)
            {
                var task = _chatServiceClient.DeleteUser(user.Username);
                tasks.Add(task);
            }
            await Task.WhenAll(tasks);
        }

        [Fact]
        public async Task PostGetMessage()
        {
            var conversationId = await AddRandomConversation();
            var message = CreateRandomMessage();
            var item = ToGetMessageResponseItem(message);
            await AddMessage(conversationId, message);
            var fetchedMessage = await _chatServiceClient.GetMessages(conversationId, 20, 0);
            Assert.Equal(item, fetchedMessage.Messages[0]);
        }

        [Fact]
        public async Task GetMessagesPaging()
        {
            var conversationId = await AddRandomConversation();//Creates a conversation and adds one message
            var messages = new List<GetMessagesResponseItem>();
            var tasks = new List<Task>();
            for (int i = 0; i < 5; ++i)
            {
                Message message = CreateRandomMessage();
                var item = ToGetMessageResponseItem(message);
                messages.Add(item);
                tasks.Add(AddMessage(conversationId, message));
            }
            await Task.WhenAll(tasks); // add all the messages in parallel

            // get first page
            GetMessagesResponse response = await _chatServiceClient.GetMessages(conversationId, 3, 0);
            Assert.Equal(3, response.Messages.Count);
            Assert.NotEmpty(response.NextUri);
            
            foreach (var message in response.Messages)
            {
                Assert.Contains(message, messages);
            }
            messages.RemoveAll(s => response.Messages.Contains(s));

            // get all the rest
            response = await _chatServiceClient.GetMessagesByUri(response.NextUri);
            Assert.Equal(3, response.Messages.Count);
            response.Messages.RemoveAt(response.Messages.Count - 1); //Removing the first message added with the conversation
            Assert.Null(response.NextUri);
            foreach (var message in response.Messages)
            {
                Assert.Contains(message, messages);
            }
        }

        [Fact]
        public async Task GetMessagesNewerThanLastSeenMessageTime()
        {
            var conversationId = await AddRandomConversation(); //Creates a conversation and adds one message
            //sending the first two messages
            var messages = new List<GetMessagesResponseItem>();
            var tasks = new List<Task>();
            for (int i = 0; i < 2; ++i)
            {
                Message message = CreateRandomMessage();
                var item = ToGetMessageResponseItem(message);
                messages.Add(item);
                tasks.Add(AddMessage(conversationId, message));
            }
            await Task.WhenAll(tasks);

            GetMessagesResponse response = await _chatServiceClient.GetMessages(conversationId, 5, 0);
            Assert.Equal(3, response.Messages.Count);
            response.Messages.RemoveAt(response.Messages.Count - 1); //Removing the first message added with the conversation
            foreach (var message in response.Messages)
            {
                Assert.Contains(message, messages);
            }
            var lastSeenMessageTime = response.Messages[0].UnixTime; //Unix time of the newest message
            messages.Clear();

            //sending two new messages
            for (int i = 0; i < 2; ++i)
            {
                Message message = CreateRandomMessage();
                var item = ToGetMessageResponseItem(message);
                messages.Add(item);
                tasks.Add(AddMessage(conversationId, message));
            }
            await Task.WhenAll(tasks);

            response = await _chatServiceClient.GetMessages(conversationId, 10, lastSeenMessageTime);
            Assert.Equal(2, response.Messages.Count); //should return 2 messages (not 5)
            foreach (var message in response.Messages)
            {
                Assert.Contains(message, messages);
            }
        }

        [Fact]
        public async Task GetMessageWithLastSeenMessageTimeAndPaging()
        {
            var conversationId = await AddRandomConversation();
            var messages = new List<GetMessagesResponseItem>();
            var tasks = new List<Task>();
            long currentUnixTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            for (int i = 0; i < 10; ++i)
            {
                Message message = CreateRandomMessage();
                var item = ToGetMessageResponseItem(message);
                messages.Add(item);
                tasks.Add(AddMessage(conversationId, message));
            }
            await Task.WhenAll(tasks);
            //there are 11 messages in the conversation, this will skip the first message (added with the conversation)
            GetMessagesResponse response = await _chatServiceClient.GetMessages(conversationId, 6 , currentUnixTime); 
            Assert.Equal(6, response.Messages.Count);
            foreach (var message in response.Messages)
            {
                Assert.Contains(message, messages);
            }
            messages.RemoveAll(s => response.Messages.Contains(s));

            // get all the rest
            response = await _chatServiceClient.GetMessagesByUri(response.NextUri);
            Assert.Equal(4, response.Messages.Count);
            Assert.Null(response.NextUri);
            foreach (var message in response.Messages)
            {
                Assert.Contains(message, messages);
            }
        }

        [Theory]
        [InlineData("conversationId", "", "senderid", "text")]
        [InlineData("conversationId", " ", "senderid", "text")]
        [InlineData("conversationId", null, "senderid", "text")]
        [InlineData("conversationId", "msgId", "", "text")]
        [InlineData("conversationId", "msgId", " ", "text")]
        [InlineData("conversationId", "msgId", null, "text")]
        [InlineData("conversationId", "msgId", "senderid", "")]
        [InlineData("conversationId", "msgId", "senderid", " ")]
        [InlineData("conversationId", "msgId", "senderId", null)]
        [InlineData(" ", "msgId", "senderid", "text")]
        public async Task SendInvalidMessageRequest(string conversationId, string messageId, string senderUsername, string text)
        {
            var message = new Message
            {
                Id = messageId,
                Text = text,
                SenderUsername = senderUsername
            };
            var e = await Assert.ThrowsAnyAsync<ChatServiceException>(() => _chatServiceClient.AddMessage(conversationId, message));
            Assert.Equal(HttpStatusCode.BadRequest, e.StatusCode);
        }
        
        [Fact]
        public async Task PostGetUpdateConversation()
        {
            //Creating a conversation adds a conversation to both particpants
            string conversationId = await AddRandomConversation();
            List<string> participants = GetParticipants(conversationId);
            await AddUser(CreateUser(participants[0]));
            await AddUser(CreateUser(participants[1]));
            var participant1FetchedConversation1 = await _chatServiceClient.GetConversations(participants[0], 20, 0);
            Assert.Equal(participant1FetchedConversation1.Conversations[0].Id, conversationId);
            Assert.Equal(participants[1], participant1FetchedConversation1.Conversations[0].Recipient.Username);
            var participant2FetchedConversation1 = await _chatServiceClient.GetConversations(participants[1], 20, 0);
            Assert.Equal(participant2FetchedConversation1.Conversations[0].Id, conversationId);
            Assert.Equal(participants[0], participant2FetchedConversation1.Conversations[0].Recipient.Username);

            //Adding a message updates LastModifiedUnixTime for both participants
            var message = CreateRandomMessage();
            var item = ToGetMessageResponseItem(message);
            await AddMessage(conversationId, message);
            var fetchedMessage = await _chatServiceClient.GetMessages(conversationId, 20, 0);
            var participant1FetchedConversation2 = await _chatServiceClient.GetConversations(participants[0], 20, 0);
            Assert.Equal(participant1FetchedConversation2.Conversations[0].LastMofiedUnixTime, fetchedMessage.Messages[0].UnixTime);
            var participant2FetchedConversation2 = await _chatServiceClient.GetConversations(participants[1], 20, 0);
            Assert.Equal(participant2FetchedConversation2.Conversations[0].LastMofiedUnixTime, fetchedMessage.Messages[0].UnixTime);
        }

        [Fact]
        public async Task GetConversationsPaging()
        {
            var username = CreateRandomString();
            await AddUser(CreateUser(username));
            List<string> conversations = new List<string>();
            for(int i=0; i<5; i++)
            {
                var conversationId = await AddRandomConversationForUser(username);
                conversations.Add(conversationId);
            }
            GetConversationsResponse response = await _chatServiceClient.GetConversations(username, 3, 0);
            Assert.Equal(3,response.Conversations.Count);
            List<string> resultConversations = new List<string>();
            foreach(var conversation in response.Conversations)
            {
                Assert.Contains(conversation.Id, conversations);
                resultConversations.Add(conversation.Id);
            }
            conversations.RemoveAll(s => resultConversations.Contains(s));

            response = await _chatServiceClient.GetConversationsByUri(response.NextUri);
            Assert.Equal(2, response.Conversations.Count);
            foreach (var conversation in response.Conversations)
            {
                Assert.Contains(conversation.Id, conversations);
            }
        }
        [Fact]
        public async Task GetConversationsNewerThanLastSeenConversationTime()
        {
            var username = CreateRandomString();
            await AddUser(CreateUser(username));
            List<string> conversations = new List<string>();
            for (int i = 0; i < 2; i++)
            {
                var conversationId = await AddRandomConversationForUser(username);
                conversations.Add(conversationId);
            }
            GetConversationsResponse response = await _chatServiceClient.GetConversations(username, 5, 0);
            Assert.Equal(2, response.Conversations.Count);
            foreach (var conversation in response.Conversations)
            {
                Assert.Contains(conversation.Id, conversations);
            }
            conversations.Clear();

            var lastSeenConversationTime = response.Conversations[0].LastMofiedUnixTime;

            for (int i = 0; i < 2; i++)
            {
                var conversationId = await AddRandomConversationForUser(username);
                conversations.Add(conversationId);
            }
            response = await _chatServiceClient.GetConversations(username, 5, lastSeenConversationTime);
            Assert.Equal(2, response.Conversations.Count);
            foreach (var conversation in response.Conversations)
            {
                Assert.Contains(conversation.Id, conversations);
            }
        }

        [Fact]
        public async Task MessageConflictDoesNotReturn409()
        {
            var conversationId = await AddRandomConversation();
            var message = CreateRandomMessage();
            var item = ToGetMessageResponseItem(message);
            await AddMessage(conversationId, message);
            //Second Message does not return conflict 409, we do not add it to cleanup or it will throw an error when trying to delete
            await _chatServiceClient.AddMessage(conversationId, message); 
            var fetchedMessage = await _chatServiceClient.GetMessages(conversationId, 20, 0);
            Assert.Equal(item, fetchedMessage.Messages[0]);
        }

        [Fact]
        public async Task ConversationConflictDoesNotReturn409()
        {
            var conversationId = await AddRandomConversation();
            var participants = GetParticipants(conversationId);
            await AddUser(CreateUser(participants[0]));
            await AddUser(CreateUser(participants[1]));
            var message = CreateRandomMessage();
            //Trying to add the conversations again will not return 409
            AddConversationResponse response = await _chatServiceClient.AddConversation(message, participants);
            List<string> item = new List<string>() { response.Id, message.Id };
            _messagesToCleanup.Add(item);
            var fetchedConversation1 = await _chatServiceClient.GetConversations(participants[0], 5, 0);
            Assert.Equal(fetchedConversation1.Conversations[0].Id, conversationId);
            var fetchedConversation2 = await _chatServiceClient.GetConversations(participants[1], 5, 0);
            Assert.Equal(fetchedConversation2.Conversations[0].Id, conversationId);
        }

        private async Task<string> AddRandomConversation()
        {
            var message = CreateRandomMessage();
            var participants = CreateRandomParticipants();
            AddConversationResponse response = await _chatServiceClient.AddConversation(message, participants);
            _conversationsToCleanup.Add(response.Id);
            List<string> item = new List<string>();
            item.Add(response.Id);
            item.Add(message.Id);
            _messagesToCleanup.Add(item);
            return response.Id;
        }

        private async Task<string> AddRandomConversationForUser(string username)
        {
            var message = CreateRandomMessage();
            List<string> participants = new List<string>();
            participants.Add(username);
            participants.Add(CreateRandomString());
            await AddUser(CreateUser(participants[1])); 
            AddConversationResponse response = await _chatServiceClient.AddConversation(message, participants);
            _conversationsToCleanup.Add(response.Id);
            List<string> item = new List<string>();
            item.Add(response.Id);
            item.Add(message.Id);
            _messagesToCleanup.Add(item);
            return response.Id;
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

        private async Task AddMessage(string conversationId, Message message)
        {
            await _chatServiceClient.AddMessage(conversationId, message);
            List<string> item = new List<string>();
            item.Add(conversationId);
            item.Add(message.Id);
            _messagesToCleanup.Add(item);
        }

        public async Task AddUser(User user)
        {
            await _chatServiceClient.AddUser(user);
            _usersToCleanup.Add(user);
        }

        public User CreateUser(string username)
        {
            var user = new User
            {
                Username = username,
                FirstName = "anyUserFN",
                LastName = "anyUserLN",
                ProfilePictureId = CreateRandomString()
            };
            return user;
        }

        public static string CreateRandomString()
        {
            return Guid.NewGuid().ToString();
        }
        
        public Message CreateRandomMessage()
        {
            var message = new Message
            {
                Id = CreateRandomString(),
                Text = CreateRandomString(),
                SenderUsername = CreateRandomString()
            };
            return message;
        }

        public List<string> CreateRandomParticipants()
        {
            List<string> participants = new List<string>();
            participants.Add(CreateRandomString());
            participants.Add(CreateRandomString());
            return participants;
        }

        public GetMessagesResponseItem ToGetMessageResponseItem(Message message)
        {
            var item = new GetMessagesResponseItem
            {
                Text = message.Text,
                SenderUsername = message.SenderUsername,
            };
            return item;
        }
    }
}
