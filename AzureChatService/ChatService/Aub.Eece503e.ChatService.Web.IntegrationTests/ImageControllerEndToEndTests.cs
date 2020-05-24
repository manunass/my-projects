using Aub.Eece503e.ChatService.Client;
using Aub.Eece503e.ChatService.Datacontracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Aub.Eece503e.ChatService.Web.IntegrationTests
{
    public abstract class ImageControllerEndToEndTests<TFixture> : IClassFixture<TFixture>, IAsyncLifetime where TFixture : class, IEndToEndTestsFixture
    {
        private readonly IChatServiceClient _chatServiceClient;
        private readonly Random _rand = new Random();

        // a concurrent container where we keep the records that were added to storage so that we can cleanup after
        private readonly ConcurrentBag<string> _imagesToCleanup = new ConcurrentBag<string>();

        public ImageControllerEndToEndTests(TFixture fixture)
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
            foreach (var imageId in _imagesToCleanup)
            {
                // the task will be started but not initiated
                var task = _chatServiceClient.DeleteImage(imageId);

                // add the task to a list
                tasks.Add(task);
            }
            // await for all the tasks to complete. The main advantage of this approach is that the tasks
            // will run in parallel.
            await Task.WhenAll(tasks);
        }

        [Fact]
        public async Task UploadDownloadImage()
        {
            var stream = ImageStream();
            UploadImageResponse uploadresponse = await UploadImage(stream);
            DownloadImageResponse downloadresponse = await _chatServiceClient.DownloadImage(uploadresponse.ImageId);
            Assert.Equal(stream.ToArray(), downloadresponse.ImageData);
        }

        [Fact]
        public async Task DownloadNonExistingImage()
        {
            string str = Guid.NewGuid().ToString();
            var e = await Assert.ThrowsAsync<ChatServiceException>(() => _chatServiceClient.DownloadImage(str));
            Assert.Equal(HttpStatusCode.NotFound, e.StatusCode);
        }

        [Fact]
        public async Task DeleteExistingImage()
        {
            var stream = ImageStream();
            UploadImageResponse uploadresponse = await _chatServiceClient.UploadImage(stream);
            await _chatServiceClient.DeleteImage(uploadresponse.ImageId);
            var e = await Assert.ThrowsAsync<ChatServiceException>(() => _chatServiceClient.DownloadImage(uploadresponse.ImageId));
            Assert.Equal(HttpStatusCode.NotFound, e.StatusCode);
        }

        [Fact]
        public async Task DeleteNonExistingImage()
        {
            string str = Guid.NewGuid().ToString();
            var e = await Assert.ThrowsAsync<ChatServiceException>(() => _chatServiceClient.DeleteImage(str));
            Assert.Equal(HttpStatusCode.NotFound, e.StatusCode);
        }

        public async Task<UploadImageResponse> UploadImage(Stream stream)
        {
            UploadImageResponse uploadresponse = await _chatServiceClient.UploadImage(stream);
            _imagesToCleanup.Add(uploadresponse.ImageId);
            return uploadresponse;
        }

        public MemoryStream ImageStream()
        {
            string str = Guid.NewGuid().ToString();
            var bytes = Encoding.UTF8.GetBytes(str);
            return new MemoryStream(bytes);
        }
    }
}
