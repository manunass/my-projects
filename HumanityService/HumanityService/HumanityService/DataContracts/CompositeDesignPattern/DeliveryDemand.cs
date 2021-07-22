using HumanityService.DataContracts.Requests;
using HumanityService.Stores.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HumanityService.DataContracts.CompositeDesignPattern
{
    public class DeliveryDemand : IComponent
    {
        public string Id { get; set; }
        public string ProcessId { get; set; }
        public string CampaignName { get; set; }
        public string PickupUsername { get; set; }
        public string DestinationUsername { get; set; }
        public Location PickupLocation { get; set; }
        public Location DestinationLocation { get; set; }
        public string Status { get; set; }
        public string OtherInfo { get; set; }
        public long TimeWindowStart { get; set; }
        public long TimeWindowEnd { get; set; }
        public long TimeCreated { get; set; }
        public long TimeCompleted { get; set; }
        
        ITransactionStore _transactionStore;
        private readonly List<IComponent> components = new List<IComponent>();

        public DeliveryDemand()
        {
        }

        public DeliveryDemand(string processId, string campaignName, string campaignUsername, AnswerCampaignRequest request)
        {
            Id = Utils.CreateId();
            ProcessId = processId;
            CampaignName = campaignName;
            PickupUsername = request.Username;
            DestinationUsername = campaignUsername;
            Status = "Pending";
            TimeWindowStart = request.TimeWindowStart;
            TimeWindowEnd = request.TimeWindowEnd;
            OtherInfo = request.OtherInfo;
            TimeCreated = Utils.UnixTimeSeconds();
            TimeCompleted = 0;
        }

        public void AddComponent(IComponent component)
        {
            components.Add(component);
        }

        public async Task AnswerDeliveryDemand(string deliveryCode, AnswerDeliveryDemandRequest request)
        {
            Status = "InProgress";
            await Update();

            Contribution contribution = new Contribution(ProcessId, Id, deliveryCode, request);
            contribution.SetStore(_transactionStore);
            AddComponent(contribution);
            await contribution.Save();
        }

        public async Task ValidatePickup()
        {
            Status = "PickedUp";
            await Update();

            Contribution contribution = (Contribution)components.Find(x => x is Contribution && x.Status != "Cancelled");
            await contribution.SetStatusPickedUp();
        }

        public async Task ValidateDestination()
        {
            Status = "Completed";
            TimeCompleted = Utils.UnixTimeSeconds();
            await Update();

            Contribution contribution = (Contribution)components.Find(x => x is Contribution && x.Status != "Cancelled");
            await contribution.SetStatusCompleted();
        }

        public async Task Cancel()
        {
            Status = "Cancelled";
            await Update();

            foreach(var component in components)
            {
                await component.Cancel();
            }
        }

        public async Task Reset()
        {
            Status = "Pending";
            await Update();
        }

        public async Task Save()
        {
            await _transactionStore.AddDeliveryDemand(this);
            foreach (var component in components)
            {
                await component.Save();
            }
        }

        public async Task Update()
        {
            await _transactionStore.UpdateDeliveryDemand(this);
        }

        public void SetStore(ITransactionStore transactionStore)
        {
            _transactionStore = transactionStore;
            foreach (var component in components)
            {
                component.SetStore(transactionStore);
            }
        }
    }
}
