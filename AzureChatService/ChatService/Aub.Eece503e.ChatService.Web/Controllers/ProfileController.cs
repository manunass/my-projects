using Aub.Eece503e.ChatService.Datacontracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Aub.Eece503e.ChatService.Web.Services;

namespace Aub.Eece503e.ChatService.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            await _profileService.AddUser(user);
            return CreatedAtAction(nameof(Post), new { username = user.Username }, user);
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> Get(string username)
        {
            User user = await _profileService.GetUser(username);
            return Ok(user);
        }

        [HttpPut("{username}")]
        public async Task<IActionResult> Put(string username, [FromBody] UpdateUserRequestBody updateuserRequestBody)
        {
            var user = new User
            {
                Username = username,
                FirstName = updateuserRequestBody.FirstName,
                LastName = updateuserRequestBody.LastName,
                ProfilePictureId = updateuserRequestBody.ProfilePictureId
            };
            
            await _profileService.UpdateUser(username, user);
            return Ok();
        }

        [HttpDelete("{username}")]
        public async Task<IActionResult> Delete(string username)
        {
            await _profileService.DeleteUser(username);
            return Ok();
        }
    }
}