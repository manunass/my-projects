using HumanityService.DataContracts.CompositeDesignPattern;
using HumanityService.DataContracts.Requests;
using HumanityService.DataContracts.Results;
using System.Threading.Tasks;

namespace HumanityService.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<Campaign> MatchCampaign(MatchCampaignRequest request);
        Task<MatchDeliveryDemandResult> MatchDeliveryDemand(MatchDeliveryDemandRequest request);

        Task<Campaign> GetCampaign(string campaignId);
        Task<GetCampaignsResult> GetCampaigns(GetCampaignsRequest request);
        Task CreateCampaign(CreateCampaignRequest request);
        Task EditCampaign(string campaignId, EditCampaignRequest request);
        Task CancelCampaign(string campaignId);
        Task AnswerCampaign(string campaignId, AnswerCampaignRequest request);

        Task<DeliveryDemand> GetDeliveryDemand(string deliveryDemandId);
        Task<GetDeliveryDemandsResult> GetDeliveryDemands(GetDeliveryDemandsRequest request);
        Task AnswerDeliveryDemand(string deliveryDemandId, AnswerDeliveryDemandRequest request);

        Task<Contribution> GetContribution(string contributionId);
        Task<GetContributionsResult> GetContributions(GetContributionsRequest request);
        Task CancelContribution(string contributionId);
        Task ApproveContribution(string contributionId);
        Task ValidateContribution(string contributionId);

        Task<Process> GetProcess(string processId);
        Task<GetProcessesResult> GetProcesses(string campaignId);
        Task<bool> ValidateDelivery(ValidateDeliveryRequest request);
    }
}
