namespace HumanityService.DataContracts
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
    }
}
