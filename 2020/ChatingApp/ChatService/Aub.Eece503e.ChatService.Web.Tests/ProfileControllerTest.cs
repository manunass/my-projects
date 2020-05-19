using System.Net;
using System.Threading.Tasks;
using Aub.Eece503e.ChatService.Datacontracts;
using Aub.Eece503e.ChatService.Web.Store;
using Aub.Eece503e.ChatService.Web.Store.Exceptions;
using Moq;
using Xunit;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Aub.Eece503e.ChatService.Client;

namespace Aub.Eece503e.ChatService.Web.Tests
{
    public class ProfileControllerTests
    {
        private User _testUser = new User
        {
            Username = "johnny1999",
            FirstName = "John",
            LastName = "Smith",
            ProfilePictureId = "4321"
        };

        [Theory]
        [InlineData(HttpStatusCode.ServiceUnavailable)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.TooManyRequests)]
        [InlineData(HttpStatusCode.Conflict)]
        public async Task AddUserReturnsCorrectStatusCodeOnStorageErrors(HttpStatusCode statusCode)
        {
            var profileStoreMock = new Mock<IProfileStore>();
            profileStoreMock.Setup(store => store.AddUser(_testUser)).ThrowsAsync(new StorageErrorException("Test Exception", (int)statusCode));
            TestServer testServer = new TestServer(
                Program.CreateWebHostBuilder(new string[] { })
                .ConfigureTestServices(services =>
                {
                    services.AddSingleton(profileStoreMock.Object);
                }).UseEnvironment("Development"));
            var httpClient = testServer.CreateClient();
            var chatServiceClient = new ChatServiceClient(httpClient);

            ChatServiceException e = await Assert.ThrowsAsync<ChatServiceException>(() => chatServiceClient.AddUser(_testUser));
            Assert.Equal(statusCode, e.StatusCode);
        }

        [Theory]
        [InlineData(HttpStatusCode.ServiceUnavailable)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.TooManyRequests)]
        [InlineData(HttpStatusCode.Conflict)]
        public async Task GetUserReturnsCorrectStatusCodeOnStorageErrors(HttpStatusCode statusCode)
        {
            var profileStoreMock = new Mock<IProfileStore>();
            profileStoreMock.Setup(store => store.GetUser(_testUser.Username)).ThrowsAsync(new StorageErrorException("Test Exception", (int)statusCode));
            TestServer testServer = new TestServer(
                Program.CreateWebHostBuilder(new string[] { })
                .ConfigureTestServices(services =>
                {
                    services.AddSingleton(profileStoreMock.Object);
                }).UseEnvironment("Development"));
            var httpClient = testServer.CreateClient();
            var chatServiceClient = new ChatServiceClient(httpClient);

            ChatServiceException e = await Assert.ThrowsAsync<ChatServiceException>(() => chatServiceClient.GetUser(_testUser.Username));
            Assert.Equal(statusCode, e.StatusCode);
        }

        [Theory]
        [InlineData(HttpStatusCode.ServiceUnavailable)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.TooManyRequests)]
        [InlineData(HttpStatusCode.Conflict)]
        public async Task PutUserReturnsCorrectStatusCodeOnStorageErrors(HttpStatusCode statusCode)
        {
            var profileStoreMock = new Mock<IProfileStore>();
            profileStoreMock.Setup(store => store.UpdateUser(_testUser)).ThrowsAsync(new StorageErrorException("Test Exception", (int)statusCode));
            TestServer testServer = new TestServer(
                Program.CreateWebHostBuilder(new string[] { })
                .ConfigureTestServices(services =>
                {
                    services.AddSingleton(profileStoreMock.Object);
                }).UseEnvironment("Development"));
            var httpClient = testServer.CreateClient();
            var chatServiceClient = new ChatServiceClient(httpClient);

            ChatServiceException e = await Assert.ThrowsAsync<ChatServiceException>(() => chatServiceClient.UpdateUser(_testUser));
            Assert.Equal(statusCode, e.StatusCode);
        }

        [Theory]
        [InlineData(HttpStatusCode.ServiceUnavailable)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.TooManyRequests)]
        [InlineData(HttpStatusCode.Conflict)]
        public async Task DeleteUserReturnsCorrectStatusCodeOnStorageErrors(HttpStatusCode statusCode)
        {
            var profileStoreMock = new Mock<IProfileStore>();
            profileStoreMock.Setup(store => store.DeleteUser(_testUser.Username)).ThrowsAsync(new StorageErrorException("Test Exception", (int)statusCode));
            TestServer testServer = new TestServer(
                Program.CreateWebHostBuilder(new string[] { })
                .ConfigureTestServices(services =>
                {
                    services.AddSingleton(profileStoreMock.Object);
                }).UseEnvironment("Development"));
            var httpClient = testServer.CreateClient();
            var chatServiceClient = new ChatServiceClient(httpClient);

            ChatServiceException e = await Assert.ThrowsAsync<ChatServiceException>(() => chatServiceClient.DeleteUser(_testUser.Username));
            Assert.Equal(statusCode, e.StatusCode);
        }
    }
}
