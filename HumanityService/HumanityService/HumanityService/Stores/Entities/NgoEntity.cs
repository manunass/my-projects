namespace HumanityService.Stores
{
    [System.ComponentModel.DataAnnotations.Schema.Table(TableName)]
    public class NgoEntity
    {
        public const string TableName = "ngos";
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string RegistrationNumber { get; set; }
        public string WebsiteAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
    }
}
