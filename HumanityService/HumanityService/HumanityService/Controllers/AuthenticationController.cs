using System.Threading.Tasks;
using HumanityService.DataContracts.Requests;
using HumanityService.DataContracts.Results;
using HumanityService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HumanityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        //List of Http Methods (API)
        [HttpPost("login-user")]
        public async Task<IActionResult> LoginUser([FromBody] LoginRequest request)
        {
            var result = await _authenticationService.LoginUser(request.Username, request.Password);
            return Ok(result);
        }

        [HttpPost("login-ngo")]
        public async Task<IActionResult> LoginNgo([FromBody]  LoginRequest request)
        {
            var result = await _authenticationService.LoginNgo(request.Username, request.Password);
            return Ok(result);
        }
    }
}
