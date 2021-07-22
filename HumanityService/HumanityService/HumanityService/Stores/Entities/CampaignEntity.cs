using System.Collections.Generic;

namespace HumanityService.Stores
{
    [System.ComponentModel.DataAnnotations.Schema.Table(TableName)]
    public class CampaignEntity
    {
        public const string TableName = "campaigns";
        public string Id { get; set; }
        public string Name { get; set; }
        public string NgoName { get; set; }
        public string Username { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public int CompletedCount { get; set; }
        public long TimeCreated { get; set; }
        public long TimeCompleted { get; set; }
        public string Description { get; set; }
    }
}
