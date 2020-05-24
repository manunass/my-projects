using Aub.Eece503e.ChatService.Datacontracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Aub.Eece503e.ChatService.Web.Services;

namespace Aub.Eece503e.ChatService.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImagesController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] IFormFile file)
        {
            string imageId = await _imageService.UploadImage(file);
            return CreatedAtAction(nameof(Get),
                new { Id = imageId }, new UploadImageResponse
                {
                    ImageId = imageId
                });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            byte[] bytes = await _imageService.DownloadImage(id);
            return new FileContentResult(bytes, "application/octet-stream");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _imageService.DeleteImage(id);
            return Ok();
        }
    }
}
