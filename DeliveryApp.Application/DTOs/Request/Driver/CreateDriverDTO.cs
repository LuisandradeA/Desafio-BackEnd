namespace DeliveryApp.Application.DTOs.Request.Driver
{
    public class CreateDriverDTO
    {
        public string Identifier { get; set; }
        public string Name { get; set; }
        public string Document { get; set; }
        public DateTime BirthDate { get; set; }
        public string LicenseNumber { get; set; }
        public string LicenseType { get; set; }
    }
}
