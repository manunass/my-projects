using HumanityService.DataContracts.Requests;
using HumanityService.Stores.Interfaces;
using System.Threading.Tasks;

namespace HumanityService.DataContracts.CompositeDesignPattern
{
    public class Contribution : IComponent
    {
        public string Id { get; set; }
        public string ProcessId { get; set; }
        public string DeliveryDemandId { get; set; }
        public string DeliveryCode { get; set; }
        public string Type { get; set; }
        public string Username { get; set; }
        public string Status { get; set; }
        public long TimeWindowStart { get; set; }
        public long TimeWindowEnd { get; set; }
        public string OtherInfo { get; set; }
        public long TimeCreated { get; set; }
        public long TimeCompleted { get; set; }
        public Location Location { get; set; }

        ITransactionStore _transactionStore;

        public Contribution()
        {
        }

        public Contribution(string processId, string campaignType, AnswerCampaignRequest request)
        {
            Id = Utils.CreateId();
            ProcessId = processId;
            Type = campaignType;
            Username = request.Username;
            Status = "Pending";
            TimeWindowStart = request.TimeWindowStart;
            TimeWindowEnd = request.TimeWindowEnd;
            OtherInfo = request.OtherInfo;
            TimeCreated = Utils.UnixTimeSeconds();
            TimeCompleted = 0;
        }

        public Contribution(string processId, string deliveryDemandId, string deliveryCode, AnswerDeliveryDemandRequest request)
        {
            Id = Utils.CreateId();
            ProcessId = processId;
            DeliveryDemandId = deliveryDemandId;
            DeliveryCode = deliveryCode;
            Type = "Delivery";
            Username = request.Username;
            Status = "InProgress";
            TimeWindowStart = request.TimeWindowStart;
            TimeWindowEnd = request.TimeWindowEnd;
            OtherInfo = request.OtherInfo;
            TimeCreated = Utils.UnixTimeSeconds();
            TimeCompleted = 0;
        }

        public async Task ValidatePickup()
        {
            await SetStatusPickedUp();
        }

        public async Task ValidateDestination()
        {
            await SetStatusCompleted();
            await SetTimeCompleted();
        }

        public async Task SetStatusInProgress()
        {
            Status = "InProgress";
            await Update();
        }

        public async Task SetStatusPickedUp()
        {
            Status = "PickedUp";
            await Update();
        }

        public async Task SetStatusCompleted()
        {
            Status = "Completed";
            await Update();
        }

        public async Task SetTimeCompleted()
        {
            TimeCompleted = Utils.UnixTimeSeconds();
            await Update();
        }

        public async Task Cancel()
        {
            if (Status != "Cancelled")
            {
                Status = "Cancelled";
                await Update();
            }
        }

        public async Task Reset()
        {
            Status = "Pending";
            await Update();
        }

        public async Task Save()
        {
            await _transactionStore.AddContribution(this);
        }

        public async Task Update()
        {
            await _transactionStore.UpdateContribution(this);
        }

        public void SetStore(ITransactionStore transactionStore)
        {
            _transactionStore = transactionStore;
        }
    }
}
