namespace HumanityService.DataContracts
{
    public class AnswerDeliveryDemandRequest
    {
        public string Username { get; set; }
        public long TimeWindowStart { get; set; }
        public long TimeWindowEnd { get; set; }
        public Location Location { get; set; }
        public string OtherInfo { get; set; }
    }
}
