using System.Text.Json.Serialization;

namespace DeliveryApp.Application.DTOs.Request.Rentals
{
    public class CreateRentalDTO
    {
        [JsonPropertyName("entregador_id")]
        public string DriverId { get; set; }

        [JsonPropertyName("moto_id")]
        public string MotorcycleId { get; set; }

        [JsonPropertyName("data_inicio")]
        public DateTime StartTime { get; set; }

        [JsonPropertyName("data_termino")]
        public DateTime EndTime { get; set; }

        [JsonPropertyName("data_previsao_termino")]
        public DateTime PrevisionTime { get; set; }

        [JsonPropertyName("plano")]
        public int PlanType { get; set; }
    }
}
