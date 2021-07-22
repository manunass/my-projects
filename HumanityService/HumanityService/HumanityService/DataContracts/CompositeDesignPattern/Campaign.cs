using HumanityService.DataContracts.Requests;
using HumanityService.Stores.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HumanityService.DataContracts.CompositeDesignPattern
{
    public class Campaign : IComponent
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NgoName { get; set; }
        public string Username { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public int Target { get; set; }
        public int CompletedCount { get; set; }
        public long TimeCreated { get; set; }
        public long TimeCompleted { get; set; }
        public string Description { get; set; }
        public Location Location { get; set; }
        

        private readonly List<IComponent> components = new List<IComponent>();
        ITransactionStore _transactionStore;

        public Campaign()
        {
        }

        public Campaign(CreateCampaignRequest createCampaignRequest)
        {
            Id = Utils.CreateId();
            Name = createCampaignRequest.Name;
            Username = createCampaignRequest.NgoUsername;
            NgoName = createCampaignRequest.NgoName;
            Type = createCampaignRequest.Type;
            Category = createCampaignRequest.Category;
            Status = "Active";
            CompletedCount = 0;
            TimeCreated = Utils.UnixTimeSeconds();
            TimeCompleted = 0;
            Description = createCampaignRequest.Description;
        }

        public async Task Cancel()
        {
            Status = "Inactive";
            TimeCompleted = Utils.UnixTimeSeconds();
            await Update();

            foreach(var component in components)
            {
                await component.Cancel();
            }
        }

        public async Task AnswerCampaign(AnswerCampaignRequest request)
        {
            //update the campaign with info (In the process when everything is done)
            //Add a new process
            Process process = new Process(Id, Name, Username, Type, request);
            process.SetStore(_transactionStore);
            AddComponent(process);
            await process.Save();
        }

        public void AddComponent(IComponent component)
        {
            components.Add(component);
        }

        public async Task Save()
        {
            await _transactionStore.AddCampaign(this);
        }

        public async Task Update()
        {
            await _transactionStore.UpdateCampaign(this);
        }

        public void SetStore(ITransactionStore transactionStore)
        {
            _transactionStore = transactionStore;
            foreach(var component in components)
            {
                component.SetStore(transactionStore);
            }
        }
    }
}
