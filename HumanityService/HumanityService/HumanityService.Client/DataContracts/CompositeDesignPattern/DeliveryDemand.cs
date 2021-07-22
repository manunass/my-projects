namespace HumanityService.DataContracts
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
    }
}
