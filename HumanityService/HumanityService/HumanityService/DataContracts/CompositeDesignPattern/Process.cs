using HumanityService.DataContracts.Requests;
using HumanityService.Stores.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HumanityService.DataContracts.CompositeDesignPattern
{
    public class Process : IComponent
    {
        public string Id { get; set; }
        public string CampaignId { get; set; }
        public string Status { get; set; }
        public long TimeCreated { get; set; }
        public long TimePickedUp { get; set; }
        public long TimeCompleted { get; set; }
        public string DeliveryCode { get; set; }

        ITransactionStore _transactionStore;
        private readonly List<IComponent> components = new List<IComponent>();

        public Process()
        {
        }

        public Process(string campaignId, string campaignName, string campaignUsername, string campaignType, AnswerCampaignRequest request)
        {
            //Process constructor
            Id = Utils.CreateId();
            CampaignId = campaignId;
            //Initially, a process' status is: Pending
            Status = "Pending";
            TimeCreated = Utils.UnixTimeSeconds();
            TimePickedUp = 0L;
            TimeCompleted = 0L;
            DeliveryCode = "";
            //A new contribution availabilty gets posted
            Contribution contribution = new Contribution(Id, campaignType, request);
            AddComponent(contribution);

            //A process automatically intiates a new delivery demand if the campaign is a physical donation.
            if (campaignType == "Donation")
            {
                DeliveryDemand deliveryDemand = new DeliveryDemand(Id, campaignName, campaignUsername, request);
                AddComponent(deliveryDemand);
            }
        }

        public async Task ApproveContribution() //For volunteer. Once a contribution is assigned, its status is updated to: InProgress.
        {
            Status = "InProgress";
            await Update();

            var contribution = GetContribution();
            await contribution.SetStatusInProgress();
        }

        public async Task ValidateContribution() //For volunteer. Once a contribution is completed, its status is updated to: Completed.
        {
            Status = "Completed";
            TimeCompleted = Utils.UnixTimeSeconds();
            await Update();

            var contribution = GetContribution();
            await contribution.SetStatusCompleted();
        }


        public async Task AnswerDeliveryDemand(AnswerDeliveryDemandRequest request)
        //For delivery courier. Once a delivery demand is answered, its status is updated to: InProgress.
        {
            Status = "InProgress";
            // Delivery Code is auto-generated for later valdation
            DeliveryCode = CreateDeliveryCode();
            await Update();

            var deliveryDemand = GetDeliveryDemand();
            await deliveryDemand.AnswerDeliveryDemand(DeliveryCode, request);

            var contribution = GetContribution();
            await contribution.SetStatusInProgress();
        }

        public async Task ValidatePickup()
        {
            Status = "PickedUp";
            TimePickedUp = Utils.UnixTimeSeconds();
            await Update();

            var deliveryDemand = GetDeliveryDemand();
            await deliveryDemand.ValidatePickup();

            var contribution = GetContribution();
            await contribution.ValidatePickup();
        }

        public async Task ValidateDestination()
        {
            Status = "Completed";
            TimeCompleted = Utils.UnixTimeSeconds();
            await Update();

            var deliveryDemand = GetDeliveryDemand();
            await deliveryDemand.ValidateDestination();

            var contribution = GetContribution();
            await contribution.ValidateDestination();
        }

        public async Task Cancel()
        {
            // Only cancel processes that aren't ongoing (i.e delivery demand/volunteering not accepted yet)
            if(Status == "Pending")
            {
                Status = "Cancelled";
                await Update();

                foreach (var component in components)
                {
                    await component.Cancel();
                }
            }
        }

        public async Task CancelDeliveryContribution()
        // Once a delivery contribution (answer) is canceled, its status is back to: Pending and its reset.
        {
            Status = "Pending";
            await Update();

            var deliveryDemand = GetDeliveryDemand();
            await deliveryDemand.Cancel();
            await deliveryDemand.Reset();

            var contribution = GetContribution();
            await contribution.Reset();
        }

        public DeliveryDemand GetDeliveryDemand()
        {
            var deliveryDemand = (DeliveryDemand)components.Find(x => x is DeliveryDemand);
            return deliveryDemand;
        }

        public Contribution GetContribution()
        {
            var contribution = (Contribution)components.Find(x => x is Contribution);
            return contribution;
        }

        public async Task Save()
        {
            await _transactionStore.AddProcess(this);
            foreach (var component in components)
            {
                await component.Save();
            }
        }

        public async Task Update()
        {
            await _transactionStore.UpdateProcess(this);
        }

        public void AddComponent(IComponent component)
        {
            components.Add(component);
        }

        public void SetStore(ITransactionStore transactionStore)
        {
            _transactionStore = transactionStore;
            foreach (var component in components)
            {
                component.SetStore(transactionStore);
            }
        }

        private string CreateDeliveryCode()
        {
            // ex output: "230798"
            // The code is simple to facilitate typing for donors
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < 6; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }
    }
}
