using HumanityService.DataContracts.CompositeDesignPattern;
using HumanityService.DataContracts.Requests;
using HumanityService.DataContracts.Results;
using System.Threading.Tasks;

namespace HumanityService.Stores.Interfaces
{
    public interface ITransactionStore
    {
        Task<Campaign> GetCampaign(string campaignId);
        Task<GetCampaignsResult> GetCampaigns(GetCampaignsRequest request);
        Task<string> AddCampaign(Campaign campaign);
        Task UpdateCampaign(Campaign campaign);
        Task DeleteCampaign(string campaignId);

        Task<DeliveryDemand> GetDeliveryDemand(string deliveryDemandId);
        Task<GetDeliveryDemandsResult> GetDeliveryDemands(GetDeliveryDemandsRequest request);
        Task<string> AddDeliveryDemand(DeliveryDemand deliveryDemand);
        Task UpdateDeliveryDemand(DeliveryDemand deliveryDemand);
        Task DeleteDeliveryDemand(string deliveryDemandId);

        Task<Contribution> GetContribution(string contributionId);
        Task<GetContributionsResult> GetContributions(GetContributionsRequest request);
        Task<string> AddContribution(Contribution contribution);
        Task UpdateContribution(Contribution request);
        Task DeleteContribution(string contributionId);

        Task<Process> GetProcess(string processId);
        Task<GetProcessesResult> GetProcesses(string campaignId);
        Task<string> AddProcess(Process process);
        Task UpdateProcess(Process process);
        Task DeleteProcess(string processId);
    }
}
