namespace GreenCoinService.DataContracts.Entities
{
    public class AddressEntity
    {
        public string Id { get; set; }
        public string MunicipalityId { get; set; }
        public MunicipalityEntity Municipality { get; set; }
        public string BusinessId { get; set; }
        public BusinessEntity Business { get; set; }
        public string UserId { get; set; }
        public UserEntity User { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string AreaOrStreet { get; set; }
        public string Building { get; set; }
        public string Flat { get; set; }
    }
}
