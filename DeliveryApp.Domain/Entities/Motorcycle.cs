namespace DeliveryApp.Domain.Entities
{
    public class Motorcycle
    {
        public Guid Identifier { get; set; }
        public int Year { get; set; }
        public string ModelName { get; set; }
        public string LicensePlate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedTime { get;} = DateTime.UtcNow;

        public Motorcycle(Guid identifier, int year, string modelName, string licensePlate)
        {
            Identifier = identifier;
            Year = year;
            ModelName = modelName;
            LicensePlate = licensePlate;
            IsDeleted = false;
        }
    }
}
