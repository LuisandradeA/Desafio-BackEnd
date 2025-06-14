namespace DeliveryApp.Application.DTOs.Messaging
{
    public class CreatedMotorcycleEventMessage
    {
        public string Id { get; set; }
        public string  ModelName { get; set; }
        public int Year { get; set; }
        public string LicensePlate { get; set; }
    }
}
