using System.Threading.Tasks;
using AutoMapper;
using GreenCoinService.DataContracts;
using GreenCoinService.DataContracts.Requests;
using GreenCoinService.DataContracts.Responses;
using GreenCoinService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace GreenCoinService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUsersService _userService;
        private readonly IOperationsService _operationsService;
        private readonly IMapper _mapper;

        public UsersController (IUsersService userService, IOperationsService operationsService)
        {
            _userService = userService;
            _operationsService = operationsService;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateUserRequest, User>();
                cfg.CreateMap<UpdateUserRequest, User>();
            });
            _mapper = configuration.CreateMapper();
        }

        /// <summary>
        /// Fetch User. Can be used by users and municipalities.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(User), 200)]
        public async Task <IActionResult> Get(string userId)
        {
            var user = await _userService.GetUser(userId);
            return Ok(user);
        }

        /// <summary>
        /// Add user. Can be used by users on signup.
        /// This will create a wallet and a waste code for each recyclable that are linked to this user.
        /// </summary>
        /// <param name="createUserRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(User), 201)]
        public async Task <IActionResult> Add([FromBody] CreateUserRequest createUserRequest)
        {
            var user = _mapper.Map<User>(createUserRequest);
            var recyclables = await _operationsService.GetRecyclables();
            var addUserResult = await _userService.AddUser(user, recyclables);
            return CreatedAtAction(nameof(Get), new { userId = addUserResult.Id }, addUserResult);
        }

        /// <summary>
        /// Update User. Can be used by users to modify their information.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="updateUserRequest"></param>
        /// <returns></returns>
        [HttpPut("{userId}")]
        public async Task <IActionResult> Update(string userId, [FromBody] UpdateUserRequest updateUserRequest)
        {
            var user = await _userService.GetUser(userId);
            UpdateUserProperties(user, updateUserRequest);
            await _userService.UpdateUser(user);
            return Ok();
        }

        /// <summary>
        /// Delete a user. Can be used by users to delete their account.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete("{userId}")]
        public async Task <IActionResult> Delete(string userId)
        {
            await _userService.DeleteUser(userId);
            return Ok();
        }

        /// <summary>
        /// Search users. Can be used by municipality admins.
        /// </summary>
        /// <param name="municipalityId"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        [HttpGet()]
        [ProducesResponseType(typeof(SearchUsersResponse), 200)]
        public async Task<IActionResult> Search(string municipalityId, string firstName = null, string lastName = null)
        {
            var users = await _userService.SearchUsers(municipalityId, firstName, lastName);
            var searchUsersResponse = new SearchUsersResponse
            {
                Users = users
            };
            return Ok(searchUsersResponse);
        }

        private static void UpdateUserProperties(User user, UpdateUserRequest updateUserRequest)
        {
            user.Address = updateUserRequest.Address;
            user.Email = updateUserRequest.Email;
        }
    }
}
