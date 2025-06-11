using System.Text.Json.Serialization;

namespace DeliveryApp.Application.DTOs.Request
{
    public class UpdateLicensePlateDTO
    {
        [JsonIgnore]
        public string? Id { get; set; }

        [JsonPropertyName("placa")]
        public string LicensePlate { get; set; }

        public UpdateLicensePlateDTO(string id, string licensePlate)
        {
            Id = id;
            LicensePlate = licensePlate;
        }
    }
}
