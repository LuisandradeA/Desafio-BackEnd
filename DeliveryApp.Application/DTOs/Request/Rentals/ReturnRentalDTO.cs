using System.Text.Json.Serialization;

namespace DeliveryApp.Application.DTOs.Request.Rentals
{
    public class ReturnRentalDTO
    {
        [JsonPropertyName("data_devolucao")]
        public DateTime ReturnDate { get; set; }
    }
}
