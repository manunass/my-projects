using AutoMapper;
using GreenCoinService.DataContracts;
using GreenCoinService.DataContracts.Requests;
using GreenCoinService.DataContracts.Responses;
using GreenCoinService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GreenCoinService.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestsService _requestsService;
        private readonly IMapper _mapper;

        public RequestsController(IRequestsService requestsService)
        {
            _requestsService = requestsService;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateRequestRequest, Request>();
            });
            _mapper = configuration.CreateMapper();
        }

        /// <summary>
        /// Create request. Can be used by users and businesses to request cashout from coins to LBP.
        /// </summary>
        /// <param name="createRequestRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Request), 200)]
        public async Task<IActionResult> Create([FromBody] CreateRequestRequest createRequestRequest)
        {
            var request = _mapper.Map<Request>(createRequestRequest);
            var createRequestResult = await _requestsService.AddRequest(request);
            return CreatedAtAction(nameof(Get), new { requestId = createRequestResult.Id}, createRequestResult);
        }

        /// <summary>
        /// Fetch request. Can be used by all users who own the request or by municipality admins.
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [HttpGet("{requestId}")]
        [ProducesResponseType(typeof(Request), 200)]
        public async Task<IActionResult> Get(string requestId)
        {
            var request = await _requestsService.GetRequest(requestId);
            return Ok(request);
        }

        /// <summary>
        /// Search requests. Can be used by all users given a adequate level of authorization.
        /// i.e. Users can only fetch request with their userId, businesses with their businessId, municipality admins can use any field.
        /// </summary>
        /// <param name="municipalityId">Id of the municipality to which the request was sent.</param>
        /// <param name="userId">Id of the user that submitted the request.</param>
        /// <param name="businessId">Id of the business that submitted the request.</param>
        /// <param name="status">Status of the request: Pending, Approved, Declined, Completed, Cancelled.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(SearchRequestsResponse), 200)]
        public async Task<IActionResult> Search(string municipalityId, string userId, string businessId, RequestStatus? status)
        {
            var requests = await _requestsService.SearchRequests(municipalityId, userId, businessId, status);
            var searchRequestsResponse = new SearchRequestsResponse
            {
                Requests = requests
            };
            return Ok(searchRequestsResponse);
        }

        /// <summary>
        /// Approve a request. Can be used by muncipality admins.
        /// This will set the request status to Approved.
        /// Only Pending requests can be Approved.
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [HttpPatch("{requestId}/approve")]
        public async Task<IActionResult> Approve(string requestId)
        {
            await _requestsService.ApproveRequest(requestId);
            return Ok();
        }

        /// <summary>
        /// Decline a request. Can be used by municipality admins.
        /// This will set the request status to Declined.
        /// Only Pending requests can be Declined.
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [HttpPatch("{requestId}/decline")]
        public async Task<IActionResult> Decline(string requestId)
        {
            await _requestsService.DeclineRequest(requestId);
            return Ok();
        }

        /// <summary>
        /// Cancel the request. Can be used by users or municipality admins.
        /// Only Pending or Approved requests can be Cancelled.
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [HttpPatch("{requestId}/cancel")]
        public async Task<IActionResult> Cancel(string requestId)
        {
            await _requestsService.CancelRequest(requestId);
            return Ok();
        }

        /// <summary>
        /// Complete a request. Can be used by municipality admins.
        /// This will set the user balance to 0, assuming he took the equivalent amount in LBP.
        /// Only Approved requests can be set to Completed.
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [HttpPatch("{requestId}/complete")]
        public async Task<IActionResult> Complete(string requestId)
        {
            await _requestsService.CompleteRequest(requestId);
            return Ok();
        }

        /// <summary>
        /// Delete a request. Can only be used by developers for testing purposes.
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [HttpDelete("{requestId}")]
        public async Task<IActionResult> Delete(string requestId)
        {
            await _requestsService.DeleteRequest(requestId);
            return Ok();
        }
    }
}
