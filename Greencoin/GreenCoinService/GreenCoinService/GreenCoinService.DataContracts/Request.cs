namespace GreenCoinService.DataContracts
{
    public class Request
    {
        public string Id { get; set; }
        public string BusinessId { get; set; }
        public string UserId { get; set; }
        public long UnixTimeRequested { get; set; }
        public long UnixTimeCompleted { get; set; }
        public long UnixTimeApproved { get; set; }
        public string Type { get; set; }
        public RequestStatus Status { get; set; }
    }

    public enum RequestStatus
    {
        Pending, 
        Approved,
        Declined,
        Completed,
        Cancelled
    }
}
