using Aub.Eece503e.ChatService.Datacontracts;
using Aub.Eece503e.ChatService.Web.Exceptions;
using Aub.Eece503e.ChatService.Web.Store;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Aub.Eece503e.ChatService.Web.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileStore _profileStore;
        private readonly ILogger<ProfileService> _logger;
        private readonly TelemetryClient _telemetryClient;

        public ProfileService(IProfileStore profileStore, ILogger<ProfileService> logger, TelemetryClient telemetryClient)
        {
            _profileStore = profileStore;
            _logger = logger;
            _telemetryClient = telemetryClient;
        }

        public async Task AddUser(User user)
        {
            using (_logger.BeginScope("{username}", user.Username))
            {
                ThrowBadRequestIfUserIsInvalid(user);

                var stopWatch = Stopwatch.StartNew();
                await _profileStore.AddUser(user);

                _telemetryClient.TrackMetric("ProfileStore.CreateUser.Time", stopWatch.ElapsedMilliseconds);
                _telemetryClient.TrackEvent("UserCreated");
            }
        }

        public async Task<User> GetUser(string username)
        {
            using (_logger.BeginScope("{username}", username))
            {
                var stopWatch = Stopwatch.StartNew();
                User user = await _profileStore.GetUser(username);
                _telemetryClient.TrackMetric("AzureTableProfileStore.GetUser.Time", stopWatch.ElapsedMilliseconds);
                return user;
            }
        }

        public async Task UpdateUser(string username, User updatedUser)
        {
            using (_logger.BeginScope("{username}", username))
            {
                ThrowBadRequestIfUserIsInvalid(updatedUser);
                var stopWatch = Stopwatch.StartNew();
                await _profileStore.UpdateUser(updatedUser);
                _telemetryClient.TrackMetric("AzureTableProfileStore.UpdateUser.Time", stopWatch.ElapsedMilliseconds);
                _telemetryClient.TrackEvent("UserUpdated");
            }
        }

        public async Task DeleteUser(string username)
        {
            using (_logger.BeginScope("{username}", username))
            {
                var stopWatch = Stopwatch.StartNew();
                await _profileStore.DeleteUser(username);
                _telemetryClient.TrackMetric("AzureTableProfileStore.DeleteUser.Time", stopWatch.ElapsedMilliseconds);
                _telemetryClient.TrackEvent("UserDeleted");
            }
        }

        private void ThrowBadRequestIfUserIsInvalid(User user)
        {
            string error = null;
            if (string.IsNullOrWhiteSpace(user.Username))
            {
                error = "The username must not be empty";
            }
            if (string.IsNullOrWhiteSpace(user.FirstName))
            {
                error = "The First Name must not be empty";
            }
            if (string.IsNullOrWhiteSpace(user.LastName))
            {
                error = "The Last Name must not be empty";
            }

            if (error != null)
            {
                throw new BadRequestException(error);
            }
        }
    }
}
