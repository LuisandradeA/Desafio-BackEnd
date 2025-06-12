using System.Text.Json.Serialization;

namespace DeliveryApp.Application.DTOs.Request.Driver
{
    public class UploadLicenseDTO
    {
        [JsonPropertyName("imagem_cnh")]
        public string Base64Image { get; set; }
    }
}
