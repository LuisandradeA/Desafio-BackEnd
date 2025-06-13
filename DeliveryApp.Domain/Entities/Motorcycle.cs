using System.Xml.Serialization;

namespace DeliveryApp.Domain.Entities
{
    public class Motorcycle
    {
        public string Identifier { get; }
        public int Year { get; }
        public string ModelName { get; }
        public string LicensePlate { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime CreatedTime { get; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<Rentals> Rentals { get; set; }

        public Motorcycle(string identifier, int year, string modelName, string licensePlate)
        {
            Identifier = identifier;
            Year = year;
            ModelName = modelName;
            LicensePlate = licensePlate;
            IsDeleted = false;
        }

        public void SetLicensePlate(string licensePlate)
        {
            LicensePlate = licensePlate;
        }

        public void DeleteMotorcycle()
        {
            IsDeleted = true;
        }
    }
}
