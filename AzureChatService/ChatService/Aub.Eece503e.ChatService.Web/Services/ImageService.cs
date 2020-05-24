using Aub.Eece503e.ChatService.Web.Store;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Aub.Eece503e.ChatService.Web.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageStore _imageStore;
        private readonly ILogger<ImageService> _logger;
        private readonly TelemetryClient _telemetryClient;

        public ImageService(IImageStore imageStore, ILogger<ImageService> logger, TelemetryClient telemetryClient)
        {
            _imageStore = imageStore;
            _logger = logger;
            _telemetryClient = telemetryClient;
        }

        public async Task<string> UploadImage(IFormFile file)
        {
            using (var stream = new MemoryStream())
            {
                var stopWatch = Stopwatch.StartNew();
                await file.CopyToAsync(stream);
                string imageId = await _imageStore.Upload(stream.ToArray());
                _telemetryClient.TrackMetric("ImageStore.Time", stopWatch.ElapsedMilliseconds);
                _telemetryClient.TrackEvent("ImageAdded");
                return imageId;
            }
        }

        public async Task<byte[]> DownloadImage(string id)
        {
            using (_logger.BeginScope("{ImageId}", id))
            {
                var stopWatch = Stopwatch.StartNew();
                byte[] bytes = await _imageStore.Download(id);
                _telemetryClient.TrackMetric("ImageStore.Time", stopWatch.ElapsedMilliseconds);
                return bytes;
            }
        }

        public async Task DeleteImage(string id)
        {
            using (_logger.BeginScope("{ImageId}", id))
            {
                var stopWatch = Stopwatch.StartNew();
                await _imageStore.Delete(id);
                _telemetryClient.TrackMetric("ImageStore.Time", stopWatch.ElapsedMilliseconds);
                _telemetryClient.TrackEvent("ImageDeleted");
            }
        }
    }
}
