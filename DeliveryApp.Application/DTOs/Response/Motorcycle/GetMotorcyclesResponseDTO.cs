using System.Text.Json.Serialization;

namespace DeliveryApp.Application.DTOs.Response.Motorcycle
{
    public class GetMotorcyclesResponseDTO
    {
        [JsonPropertyName("identificador")]
        public string Identifier { get; set; }

        [JsonPropertyName("ano")]
        public int Year { get; set; }

        [JsonPropertyName("modelo")]
        public string ModelName { get; set; }

        [JsonPropertyName("placa")]
        public string LicensePlate { get; set; }
    }
}
