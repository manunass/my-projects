using Aub.Eece503e.ChatService.Client;
using Aub.Eece503e.ChatService.Datacontracts;
using Aub.Eece503e.ChatService.Web.Store;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Aub.Eece503e.ChatService.Web.IntegrationTests
{
    public class ConversationControllerIntegrationTests : ConversationControllerEndToEndTests<IntegrationTestFixture>
    {
        private readonly IChatServiceClient _chatServiceClient;
        private readonly IConversationStore _conversationStore;
        public ConversationControllerIntegrationTests(IntegrationTestFixture fixture) : base (fixture)
        {
            _chatServiceClient = fixture.ChatServiceClient;
            _conversationStore = fixture.ConversationStore;
        }

        [Fact]
        public async Task AddConversationFailedToAddMessage()
        {
            List<string> participants = CreateRandomParticipants();
            var conversationId = participants[0] + "_" + participants[1];
            await AddUser(CreateUser(participants[0]));
            await AddUser(CreateUser(participants[1]));
            //Simulating Add Message failure when conversation is created: no message is added
            await _conversationStore.AddConversation(conversationId, participants, 0);
            _conversationsToCleanup.Add(conversationId);
            //Retrying should not throw an error
            var firstMessage = CreateRandomMessage();
            AddConversationResponse addResponse = await _chatServiceClient.AddConversation(firstMessage, participants);
            List<string> item = new List<string>() { addResponse.Id, firstMessage.Id};
            _messagesToCleanup.Add(item);
            GetMessagesResponse messageResponse = await _chatServiceClient.GetMessages(conversationId, 5, 0);
            var message = ToGetMessageResponseItem(firstMessage);
            Assert.Equal(message, messageResponse.Messages[0]);
        }

        [Fact]
        public async Task AddConversationFailedToAddtoOneUser()
        {
            List<string> participants = CreateRandomParticipants();
            var conversationId = participants[0] + "_" + participants[1];
            await AddUser(CreateUser(participants[0]));
            await AddUser(CreateUser(participants[1]));
            //Simulating Add Conversation failure when conversation is created: only adding conversation for 1 participant 
            await _conversationStore.AddConversationForOneUserTesting(conversationId, participants, 0);
            //Retrying should not throw an error
            var firstMessage = CreateRandomMessage();
            AddConversationResponse addResponse = await _chatServiceClient.AddConversation(firstMessage, participants);
            List<string> item = new List<string>() { addResponse.Id, firstMessage.Id };
            _messagesToCleanup.Add(item);
            _conversationsToCleanup.Add(conversationId);
            GetMessagesResponse messageResponse = await _chatServiceClient.GetMessages(conversationId, 5, 0);
            var message = ToGetMessageResponseItem(firstMessage);
            Assert.Equal(message, messageResponse.Messages[0]);
        }
    }
}
