using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GreenCoinService.Services.Interfaces;
using GreenCoinService.DataContracts.Responses;
using System.Collections.Generic;

namespace GreenCoinService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImagesService _imagesService;

        public ImagesController(IImagesService imageService)
        {
            _imagesService = imageService;
        }

        /// <summary>
        /// Post one or more images. Can be used by businesses to post new images to their profile.
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost("{businessId}")]
        [ProducesResponseType(typeof(List<string>), 201)]
        public async Task<IActionResult> Post(string businessId, [FromForm] List<IFormFile> files)
        {
            await _imagesService.UploadImages(businessId, files);
            return Ok();
        }

        /// <summary>
        /// Delete an image. Can be used bu businesses to delete an image from their collection.
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="imageId"></param>
        /// <returns></returns>
        [HttpDelete("{businessId}/{imageId}")]
        public async Task<IActionResult> Delete(string businessId, string imageId)
        {
            await _imagesService.DeleteImage(businessId, imageId);
            return Ok();
        }
    }
}
