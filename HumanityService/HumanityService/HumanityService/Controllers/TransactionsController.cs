using System.Threading.Tasks;
using HumanityService.DataContracts.Requests;
using HumanityService.DataContracts.Results;
using HumanityService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HumanityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        //List of Http Methods (API)
        [HttpGet("campaigns/{campaignId}")]
        public async Task<IActionResult> GetCampaign(string campaignId)
        {
            var campaign = await _transactionService.GetCampaign(campaignId);
            return Ok(campaign);
        }

        [HttpGet("campaigns")]
        public async Task<IActionResult> GetCampaigns(string ngoName = null, string username = null, string type = null, string category = null, string status = null)
        {
            var request = new GetCampaignsRequest
            {
                NgoName = ngoName,
                Username = username,
                Type = type,
                Category = category,
                Status = status
            };
            var campaigns = await _transactionService.GetCampaigns(request);
            return Ok(campaigns);
        }

        [HttpPost("campaigns/match")]
        public async Task<IActionResult> MatchCampaign([FromBody] MatchCampaignRequest request)
        {
            var campaign = await _transactionService.MatchCampaign(request);
            return Ok(campaign);
        }

        [HttpPost("campaigns")]
        public async Task<IActionResult> CreateCampaign([FromBody] CreateCampaignRequest request)
        {
            await _transactionService.CreateCampaign(request);
            return Ok();
        }

        [HttpPost("campaigns/answer-campaign/{campaignId}")]
        public async Task<IActionResult> AnswerCampaign(string campaignId, [FromBody] AnswerCampaignRequest request)
        {
            await _transactionService.AnswerCampaign(campaignId, request);
            return Ok();
        }

        [HttpPut("campaigns/{campaignId}")]
        public async Task<IActionResult> EditCampaign(string campaignId, [FromBody] EditCampaignRequest request)
        {
            await _transactionService.EditCampaign(campaignId, request);
            return Ok();
        }

        [HttpDelete("campaigns/{campaignId}")]
        public async Task<IActionResult> CancelCampaign(string campaignId)
        {
            await _transactionService.CancelCampaign(campaignId);
            return Ok();
        }

        [HttpGet("delivery-demands/{deliveryDemandId}")]
        public async Task<IActionResult> GetDeliveryDemand(string deliveryDemandId)
        {
            var deliveryDemand = await _transactionService.GetDeliveryDemand(deliveryDemandId);
            return Ok(deliveryDemand);
        }

        [HttpGet("delivery-demands")]
        public async Task<IActionResult> GetDeliveryDemands(string processId = null, string campaignName = null, string pickupUsername = null, string destinationUsername = null, string status = null)
        {
            var request = new GetDeliveryDemandsRequest
            {
                ProcessId = processId,
                CampaignName = campaignName,
                PickupUsername = pickupUsername,
                DestinationUsername = destinationUsername,
                Status = status
            };
            var deliveryDemands = await _transactionService.GetDeliveryDemands(request);
            return Ok(deliveryDemands);
        }

        [HttpPost("delivery-demands/match")]
        public async Task<IActionResult> MatchDeliveryDemand([FromBody] MatchDeliveryDemandRequest request)
        {
            var matchDeliveryDemandResult = await _transactionService.MatchDeliveryDemand(request);
            return Ok(matchDeliveryDemandResult);
        }

        [HttpPost("delivery-demands/answer-delivery-demand/{deliveryDemandId}")]
        public async Task<IActionResult> AnswerDeliveryDemand(string deliveryDemandId, [FromBody] AnswerDeliveryDemandRequest request)
        {
            await _transactionService.AnswerDeliveryDemand(deliveryDemandId, request);
            return Ok();
        }

        [HttpGet("contributions/{contributionId}")]
        public async Task<IActionResult> GetContribution(string contributionId)
        {
            var contribution = await _transactionService.GetContribution(contributionId);
            return Ok(contribution);
        }

        [HttpGet("contributions")]
        public async Task<IActionResult> GetContributions(string username = null, string processId = null, string deliveryDemandId = null, string type = null, string status = null)
        {
            var request = new GetContributionsRequest
            {
                Username = username,
                ProcessId = processId,
                DeliveryDemandId = deliveryDemandId,
                Type = type,
                Status = status
            };
            var contributions = await _transactionService.GetContributions(request);
            return Ok(contributions);
        }

        [HttpPost("contributions/approve/{contributionId}")]
        public async Task<IActionResult> ApproveContribution(string contributionId)
        {
            await _transactionService.ApproveContribution(contributionId);
            return Ok(); 
        }

        [HttpPost("contributions/validate/{contributionId}")]
        public async Task<IActionResult> ValidateContribution(string contributionId)
        {
            await _transactionService.ValidateContribution(contributionId);
            return Ok();
        }

        [HttpDelete("contributions/{contributionId}")]
        public async Task<IActionResult> CancelContribution(string contributionId)
        {
            await _transactionService.CancelContribution(contributionId);
            return Ok();
        }

        [HttpGet("processes/{processId}")]
        public async Task<IActionResult> GetProcess(string processId)
        {
            var process = await _transactionService.GetProcess(processId);
            return Ok(process);
        }

        [HttpGet("processes")]
        public async Task<IActionResult> GetProcesses(string campaignId)
        {
            var processes = await _transactionService.GetProcesses(campaignId);
            return Ok(processes);
        }

        [HttpPost("validate-delivery")]
        public async Task<IActionResult> ValidateDelivery([FromBody] ValidateDeliveryRequest request)
        {
            var isValid = await _transactionService.ValidateDelivery(request);
            var response = new ValidateDeliveryResult
            {
                IsValid = isValid
            };
            return Ok(response);
        }
    }
}
