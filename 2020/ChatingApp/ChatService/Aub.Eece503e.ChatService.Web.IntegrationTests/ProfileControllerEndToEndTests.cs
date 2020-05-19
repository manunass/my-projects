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
    public abstract class ProfileControllerEndToEndTests<TFixture> : IClassFixture<TFixture>, IAsyncLifetime where TFixture :class, IEndToEndTestsFixture
    {
        private readonly IChatServiceClient _chatServiceClient;
        private readonly Random _rand = new Random();

        // a concurrent container where we keep the records that were added to storage so that we can cleanup after
        private readonly ConcurrentBag<User> _usersToCleanup = new ConcurrentBag<User>();

        public ProfileControllerEndToEndTests(TFixture fixture)
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
            foreach (var user in _usersToCleanup)
            {
                // the task will be started but not initiated
                var task = _chatServiceClient.DeleteUser(user.Username);

                // add the task to a list
                tasks.Add(task);
            }
            // await for all the tasks to complete. The main advantage of this approach is that the tasks
            // will run in parallel.
            await Task.WhenAll(tasks);
        }

        [Fact]
        public async Task PostGetUser()
        {
            var user = CreateRandomUser();
            await AddUser(user);
            var fetchedUser = await _chatServiceClient.GetUser(user.Username);
            Assert.Equal(user, fetchedUser);
        }

        [Theory]
        [InlineData("", "Joe", "Smith", "4321")]
        [InlineData(" ", "Joe", "Smith", "4321")]
        [InlineData(null, "Joe", "Smith", "4321")]
        [InlineData("123", "", "Smith", "4321")]
        [InlineData("123", " ", "Smith", "4321")]
        [InlineData("123", null, "Smith", "4321")]
        [InlineData("123", "Joe", "", "4321")]
        [InlineData("123", "Joe", null, "4321")]
        [InlineData("123", "Joe", null, "")]

        public async Task PostInvalidUser(string username, string firstname, string lastname, string profilepictureid)
        {
            var user = new User
            {
                Username = username,
                FirstName = firstname,
                LastName = lastname,
                ProfilePictureId = profilepictureid
            };
            var e = await Assert.ThrowsAsync<ChatServiceException>(() => _chatServiceClient.AddUser(user));
            Assert.Equal(HttpStatusCode.BadRequest, e.StatusCode);
        }

        [Fact]
        public async Task GetNonExistingUser()
        {
            string randomId = CreateRandomString();
            var e = await Assert.ThrowsAsync<ChatServiceException>(() => _chatServiceClient.GetUser(randomId));
            Assert.Equal(HttpStatusCode.NotFound, e.StatusCode);
        }

        [Fact]
        public async Task AddUserThatAlreadyExists()
        {
            var user = CreateRandomUser();
            await AddUser(user);
            var e = await Assert.ThrowsAsync<ChatServiceException>(() => _chatServiceClient.AddUser(user));
            Assert.Equal(HttpStatusCode.Conflict, e.StatusCode);
        }


        [Fact]
        public async Task UpdateExistingUser()
        {
            var user = CreateRandomUser();
            await AddUser(user);
            user.FirstName = CreateRandomString();
            user.ProfilePictureId = CreateRandomString();
            await _chatServiceClient.UpdateUser(user);
            var fetchedUser = await _chatServiceClient.GetUser(user.Username);
            Assert.Equal(user, fetchedUser);
        }

        [Fact]
        public async Task UpdateNonExistingUser()
        {
            var user = CreateRandomUser();
            var e = await Assert.ThrowsAsync<ChatServiceException>(() => _chatServiceClient.UpdateUser(user));
            Assert.Equal(HttpStatusCode.NotFound, e.StatusCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]

        public async Task UpdateUserWithInvalidProperties(string name)
        {
            var user = CreateRandomUser();
            await AddUser(user);
            user.FirstName = name;
            var e = await Assert.ThrowsAsync<ChatServiceException>(() => _chatServiceClient.UpdateUser(user));
            Assert.Equal(HttpStatusCode.BadRequest, e.StatusCode);
        }

        [Fact]
        public async Task DeleteUser()
        {
            var user = CreateRandomUser();
            await _chatServiceClient.AddUser(user);
            await _chatServiceClient.DeleteUser(user.Username);
            var e = await Assert.ThrowsAsync<ChatServiceException>(() => _chatServiceClient.GetUser(user.Username));
            Assert.Equal(HttpStatusCode.NotFound, e.StatusCode);
        }

        [Fact]
        public async Task DeleteNonExistingUser()
        {
            var user = CreateRandomUser();
            var e = await Assert.ThrowsAsync<ChatServiceException>(() => _chatServiceClient.DeleteUser(user.Username));
            Assert.Equal(HttpStatusCode.NotFound, e.StatusCode);
        }

        private async Task AddUser(User user)
        {
            await _chatServiceClient.AddUser(user);
            _usersToCleanup.Add(user);
        }

        private static string CreateRandomString()
        {
            return Guid.NewGuid().ToString();
        }

        private User CreateRandomUser()
        {
            string username = CreateRandomString();
            var user = new User
            {
                Username = CreateRandomString(),
                FirstName = "anyUserFN",
                LastName = "anyUserLN",
                ProfilePictureId = CreateRandomString()
            };
            return user;
        }

    }
}
