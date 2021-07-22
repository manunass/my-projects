using System.Threading.Tasks;
using HumanityService.DataContracts.CompositeDesignPattern;
using HumanityService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HumanityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        //List of Http Methods (API)
        [HttpPost("users/signup")]
        public async Task<IActionResult> SignUp([FromBody] User user)
        {
            await _userService.AddUser(user);
            return Ok();
        }

        [HttpGet("users/{username}")]
        public async Task<IActionResult> GetUserInfo(string username)
        {
            var user = await _userService.GetUser(username);
            var userInfo = new UserInfo
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Location = user.Location
            };
            return Ok(userInfo);
        }

        [HttpDelete("users/{username}")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            await _userService.DeleteUser(username);
            return Ok();
        }

        [HttpPut("users/{username}")]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            await _userService.UpdateUser(user);
            return Ok();
        }

        [HttpPost("ngos/signup")]
        public async Task<IActionResult> SignUp([FromBody] Ngo ngo)
        {
            await _userService.AddNgo(ngo);
            return Ok();
        }

        [HttpGet("ngos/{username}")]
        public async Task<IActionResult> GetNgoInfo(string username)
        {
            var ngo = await _userService.GetNgo(username);
            var ngoInfo = new NgoInfo
            {
                Name = ngo.Name,
                Email = ngo.Email,
                Username = ngo.Username,
                RegistrationNumber = ngo.RegistrationNumber,
                WebsiteAddress = ngo.WebsiteAddress,
                PhoneNumber = ngo.PhoneNumber,
                Description = ngo.Description,
                Location = ngo.Location
            };
            return Ok(ngoInfo);
        }

        [HttpDelete("ngos/{username}")]
        public async Task<IActionResult> DeleteNgo(string username)
        {
            await _userService.DeleteNgo(username);
            return Ok();
        }

        [HttpPut("ngos/{username}")]
        public async Task<IActionResult> UpdateNgo([FromBody] Ngo ngo)
        {
            await _userService.UpdateNgo(ngo);
            return Ok();
        }
    }
}
