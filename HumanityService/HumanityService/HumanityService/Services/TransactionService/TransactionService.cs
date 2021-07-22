using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HumanityService.DataContracts.CompositeDesignPattern;
using HumanityService.DataContracts.Requests;
using HumanityService.DataContracts.Results;
using HumanityService.Exceptions;
using HumanityService.Services.Interfaces;
using HumanityService.Stores.Interfaces;

namespace HumanityService.Services
{
    public class TransactionService : ITransactionService
    {

        private readonly ITransactionStore _transactionStore;
        private readonly INotificationService _notificationService;
        private readonly IMatchingService _matchingService;
        private readonly IUserService _userService;

        public TransactionService(ITransactionStore transactionStore, IMatchingService matchingService, IUserService userService, INotificationService notificationService)
        {
            _transactionStore = transactionStore;
            _matchingService = matchingService;
            _userService = userService;
            _notificationService = notificationService;
        }

        public async Task<Campaign> MatchCampaign(MatchCampaignRequest request)
        {
            var getCampaignsRequest = new GetCampaignsRequest
            {
                Status = "Active",
                Type = request.Type,
                Category = request.Category
            };
            var getCampaignsResult = await _transactionStore.GetCampaigns(getCampaignsRequest);
            var campaigns = getCampaignsResult.Campaigns;
            var campaign = await _matchingService.MatchUserToCampaign(campaigns, request);
            return campaign;
        }

        public async Task<MatchDeliveryDemandResult> MatchDeliveryDemand(MatchDeliveryDemandRequest request)
        {
            var getDeliveryDemandsRequest = new GetDeliveryDemandsRequest
            {
                Status = "Pending"
            };
            var getDeliveryDemandsResult = await _transactionStore.GetDeliveryDemands(getDeliveryDemandsRequest);
            var deliveryDemands = getDeliveryDemandsResult.DeliveryDemands;
            var matchDeliveryDemandResult = await _matchingService.MatchUserToDeliveryDemand(deliveryDemands, request);
            return matchDeliveryDemandResult;
        }

        public async Task AnswerCampaign(string campaignId, AnswerCampaignRequest request)
        {
            Campaign campaign = await BuildCampaign(campaignId);
            await campaign.AnswerCampaign(request);
        }

        public async Task AnswerDeliveryDemand(string deliveryDemandId, AnswerDeliveryDemandRequest request)
        {
            var deliveryDemand = await _transactionStore.GetDeliveryDemand(deliveryDemandId);
            Process process = await BuildProcess(deliveryDemand.ProcessId);
            await process.AnswerDeliveryDemand(request);
            var user = await _userService.GetUser(deliveryDemand.PickupUsername);
            _notificationService.NotifyUser(user.Email, "PickUp volunteer on his way!", request.Username + " Will be picking up your donation.");
        }

        public async Task<bool> ValidateDelivery(ValidateDeliveryRequest request) 
        {
            if (request.ValidationType == "Pickup")
            {
                var contribution = await _transactionStore.GetContribution(request.ContributionId);
                Process process = await BuildProcess(contribution.ProcessId);
                if (process.DeliveryCode == request.DeliveryCode)
                {
                    await process.ValidatePickup();
                    return true;
                }
                else return false;
            }
            else if (request.ValidationType == "Destination")
            {
                var getProcessesResult = await _transactionStore.GetProcesses(request.CampaignId);
                Process process = getProcessesResult.Processes.Find(x => x.DeliveryCode == request.DeliveryCode);
                if (process != null)
                {
                    process = await BuildProcess(process.Id);
                    await process.ValidateDestination();
                    var deliveryDemand = process.GetDeliveryDemand();
                    var user = await _userService.GetUser(deliveryDemand.PickupUsername);
                    _notificationService.NotifyUser(user.Email, "Donation Delivered!", "Hey " + user.FirstName + "! Your donation has been delivered. Thank you for your generosity!");
                    return true;
                }
                else return false;
            }
            else throw new BadRequestException("Invalid ValidationType");
        }

        public async Task ApproveContribution(string contributionId)
        {
            var contribution = await _transactionStore.GetContribution(contributionId);
            var process = await BuildProcess(contribution.ProcessId);
            await process.ApproveContribution();
            var user = await _userService.GetUser(contribution.Username);
            var campaign = await _transactionStore.GetCampaign(process.CampaignId);
            _notificationService.NotifyUser(user.Email, "Volunteering Job Approved!", campaign.NgoName + " needs your help! They will contact you soon for more details.");
        }

        public async Task ValidateContribution(string contributionId)
        {

            var contribution = await _transactionStore.GetContribution(contributionId);
            var process = await BuildProcess(contribution.ProcessId);
            await process.ValidateContribution();
            var user = await _userService.GetUser(contribution.Username);
            var campaign = await _transactionStore.GetCampaign(process.CampaignId);
            campaign.CompletedCount++;
            await _transactionStore.UpdateCampaign(campaign);
            _notificationService.NotifyUser(user.Email, "Volunteering Job Validated!", campaign.NgoName + " just validated your volunteering work. Good Job " + user.FirstName + "!");
        }

        public async Task CreateCampaign(CreateCampaignRequest request)
        {
            Campaign campaign = new Campaign(request);
            campaign.SetStore(_transactionStore);
            await campaign.Save();
        }

        public async Task CancelCampaign(string campaignId)
        {
            Campaign campaign = await BuildCampaign(campaignId);
            await campaign.Cancel();
        }

        public async Task CancelContribution(string contributionId)
        {
            var contribution = await _transactionStore.GetContribution(contributionId);
            var process = await BuildProcess(contribution.ProcessId);

            if (contribution.Type == "Delivery")
            {
                await process.CancelDeliveryContribution();
            }
            else
            {
                await process.Cancel();
            }
        }

        public async Task EditCampaign(string campaignId, EditCampaignRequest request)
        {
            var campaign = await _transactionStore.GetCampaign(campaignId);
            campaign.Name = request.CampaignName;
            campaign.Type = request.Type;
            campaign.Category = request.Category;
            campaign.Target = request.Target;
            campaign.Description = request.Description;
            campaign.Status = request.Status;
            await _transactionStore.UpdateCampaign(campaign);
        }

        public async Task<Campaign> GetCampaign(string campaignId)
        {
            var campaign = await _transactionStore.GetCampaign(campaignId);
            return campaign;
        }

        public async Task<GetCampaignsResult> GetCampaigns(GetCampaignsRequest request)
        {
            var getCampaignsResult = await _transactionStore.GetCampaigns(request);
            return getCampaignsResult;
        }

        public async Task<Contribution> GetContribution(string contributionId)
        {
            var contribution = await _transactionStore.GetContribution(contributionId);
            return contribution;
        }

        public async Task<GetContributionsResult> GetContributions(GetContributionsRequest request)
        {
            var getContributionsResult = await _transactionStore.GetContributions(request);
            return getContributionsResult;
        }

        public async Task<DeliveryDemand> GetDeliveryDemand(string deliveryDemandId)
        {
            var deliveryDemand = await _transactionStore.GetDeliveryDemand(deliveryDemandId);
            return deliveryDemand;
        }

        public async Task<GetDeliveryDemandsResult> GetDeliveryDemands(GetDeliveryDemandsRequest request)
        {
            var getDeliveryDemandsResult = await _transactionStore.GetDeliveryDemands(request);
            return getDeliveryDemandsResult;
        }

        public async Task<Process> GetProcess(string processId)
        {
            var process = await _transactionStore.GetProcess(processId);
            return process;
        }

        public async Task<GetProcessesResult> GetProcesses(string campaignId)
        {
            var getProcessesResult = await _transactionStore.GetProcesses(campaignId);
            return getProcessesResult;
        }

        private async Task<Process> BuildProcess(string processId)
        {
            Process process = await _transactionStore.GetProcess(processId);
            var getContributionsRequest = new GetContributionsRequest
            {
                ProcessId = processId
            };
            var contributions = await _transactionStore.GetContributions(getContributionsRequest);
            var donorContribution = contributions.Contributions.Find(contribution => contribution.Type != "Delivery");
            var getDeliveryDemands = new GetDeliveryDemandsRequest
            {
                ProcessId = processId
            };

            var deliveryDemands = await _transactionStore.GetDeliveryDemands(getDeliveryDemands);

            if(deliveryDemands.DeliveryDemands.Count != 0)
            {
                var deliveryDemand = deliveryDemands.DeliveryDemands[0];

                contributions.Contributions.ForEach(contribution =>
                {
                    if (contribution.Type == "Delivery")
                    {
                        deliveryDemand.AddComponent(contribution);
                    }
                });
                process.AddComponent(deliveryDemand);
            }

            process.AddComponent(donorContribution);
            process.SetStore(_transactionStore);

            return process; 
        }

        private async Task<Campaign> BuildCampaign(string campaignId)
        {
            Campaign campaign = await _transactionStore.GetCampaign(campaignId);
            GetProcessesResult getProcessesResult = await _transactionStore.GetProcesses(campaignId);
            foreach(var process in getProcessesResult.Processes)
            {
                Process processTree = await BuildProcess(process.Id);
                campaign.AddComponent(processTree);
            }
            campaign.SetStore(_transactionStore);
            return campaign;
        }
    }
}
