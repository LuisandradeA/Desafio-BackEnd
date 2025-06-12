namespace DeliveryApp.Domain.Entities
{
    public class Driver
    {
        public string Identifier { get;  }
        public string Name { get;  }
        public string Document { get;  }
        public DateTime BirthDate { get;  }
        public string LicenseNumber { get;  }
        public string LicenseType { get;  }
        public string? LicenseImagePath { get; private set; }
        public DateTime CreatedTime { get; } = DateTime.UtcNow;

        public Driver(string identifier, 
            string name, 
            string document, 
            DateTime birthDate, 
            string licenseNumber,
            string licenseType)
        {
            Identifier = identifier;
            Name = name;
            Document = document;
            BirthDate = birthDate;
            LicenseNumber = licenseNumber;
            LicenseType = licenseType;
        }

        public void SetLicenseImagePath(string licenseImagePath)
        {
            LicenseImagePath = licenseImagePath;
        }
    }
}
