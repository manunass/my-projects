namespace HumanityService.DataContracts
{
    public class MatchCampaignRequest
    {
        public string Type { get; set; }
        public string Category { get; set; }
        public string TransportationType { get; set; }
        public Location Location { get; set; }
    }
}
