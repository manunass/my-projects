

namespace HumanityService.Stores
{
    [System.ComponentModel.DataAnnotations.Schema.Table(TableName)]
    public class ProcessEntity
    {
        public const string TableName = "processes";
        public string Id { get; set; }
        public string CampaignId { get; set; }
        public string Status { get; set; }
        public long TimeCreated { get; set; }
        public long TimePickedUp { get; set; }
        public long TimeCompleted { get; set; }
        public string DeliveryCode { get; set; }
    }
}
