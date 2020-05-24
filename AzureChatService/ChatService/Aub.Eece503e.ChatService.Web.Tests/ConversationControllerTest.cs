using Aub.Eece503e.ChatService.Client;
using Aub.Eece503e.ChatService.Datacontracts;
using Aub.Eece503e.ChatService.Web.Store;
using Aub.Eece503e.ChatService.Web.Store.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Aub.Eece503e.ChatService.Web.Tests
{
    public class ConversationControllerTest
    {
        private Message _testMessage = new Message
        {
            Id = "12345",
            Text = "Merry Christmas",
            SenderUsername = "Santa"
        };

        private List<string> participants = new List<string>() { "first", "second" };
        private string conversationId = "first_second";

        [Theory]
        [InlineData(HttpStatusCode.ServiceUnavailable)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.TooManyRequests)]
        [InlineData(HttpStatusCode.Conflict)]
        public async Task AddMessageReturnsCorrectStatusCodeOnStorageErrors(HttpStatusCode statusCode)
        {
            var conversationStoreMock = new Mock<IConversationStore>();
            conversationStoreMock.Setup(store => store.AddMessage("TestConversation", _testMessage)).ThrowsAsync(new StorageErrorException("Test Exception", (int)statusCode));
            TestServer testServer = new TestServer(
                Program.CreateWebHostBuilder(new string[] { })
                .ConfigureTestServices(services =>
                {
                    services.AddSingleton(conversationStoreMock.Object);
                }).UseEnvironment("Development"));
            var httpClient = testServer.CreateClient();
            var chatServiceClient = new ChatServiceClient(httpClient);

            ChatServiceException e = await Assert.ThrowsAsync<ChatServiceException>(() => chatServiceClient.AddMessage("TestConversation", _testMessage));
            Assert.Equal(statusCode, e.StatusCode);
        }

        [Theory]
        [InlineData(HttpStatusCode.ServiceUnavailable)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.TooManyRequests)]
        [InlineData(HttpStatusCode.Conflict)]
        public async Task GetMessagesReturnsCorrectStatusCodeOnStorageErrors(HttpStatusCode statusCode)
        {
            var conversationStoreMock = new Mock<IConversationStore>();
            conversationStoreMock.Setup(store => store.GetMessages("TestConversation", null , 5, 1)).ThrowsAsync(new StorageErrorException("Test Exception", (int)statusCode));
            TestServer testServer = new TestServer(
                Program.CreateWebHostBuilder(new string[] { })
                .ConfigureTestServices(services =>
                {
                    services.AddSingleton(conversationStoreMock.Object);
                }).UseEnvironment("Development"));
            var httpClient = testServer.CreateClient();
            var chatServiceClient = new ChatServiceClient(httpClient);

            ChatServiceException e = await Assert.ThrowsAsync<ChatServiceException>(() => chatServiceClient.GetMessages("TestConversation", 5, 1));
            Assert.Equal(statusCode, e.StatusCode);
        }

        [Theory]
        [InlineData(HttpStatusCode.ServiceUnavailable)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.TooManyRequests)]
        [InlineData(HttpStatusCode.Conflict)]
        public async Task AddConversationReturnsCorrectStatusCodeOnStorageErrors(HttpStatusCode statusCode)
        {
            var conversationStoreMock = new Mock<IConversationStore>();
            conversationStoreMock.Setup(store => store.AddConversation(conversationId, participants, It.IsAny<long>())).ThrowsAsync(new StorageErrorException("Test Exception", (int)statusCode));
            TestServer testServer = new TestServer(
                Program.CreateWebHostBuilder(new string[] { })
                .ConfigureTestServices(services =>
                {
                    services.AddSingleton(conversationStoreMock.Object);
                }).UseEnvironment("Development"));
            var httpClient = testServer.CreateClient();
            var chatServiceClient = new ChatServiceClient(httpClient);

            ChatServiceException e = await Assert.ThrowsAsync<ChatServiceException>(() => chatServiceClient.AddConversation(_testMessage, participants));
            Assert.Equal(statusCode, e.StatusCode);
        }

        [Theory]
        [InlineData(HttpStatusCode.ServiceUnavailable)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.TooManyRequests)]
        [InlineData(HttpStatusCode.Conflict)]
        public async Task GetConversationsReturnsCorrectStatusCodeOnStorageErrors(HttpStatusCode statusCode)
        {
            var conversationStoreMock = new Mock<IConversationStore>();
            conversationStoreMock.Setup(store => store.GetConversations("first", null, 5, 1)).ThrowsAsync(new StorageErrorException("Test Exception", (int)statusCode));
            TestServer testServer = new TestServer(
                Program.CreateWebHostBuilder(new string[] { })
                .ConfigureTestServices(services =>
                {
                    services.AddSingleton(conversationStoreMock.Object);
                }).UseEnvironment("Development"));
            var httpClient = testServer.CreateClient();
            var chatServiceClient = new ChatServiceClient(httpClient);

            ChatServiceException e = await Assert.ThrowsAsync<ChatServiceException>(() => chatServiceClient.GetConversations("first", 5, 1));
            Assert.Equal(statusCode, e.StatusCode);
        }
    }
}
