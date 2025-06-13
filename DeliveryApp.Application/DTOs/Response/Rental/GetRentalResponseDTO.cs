using System.Text.Json.Serialization;

namespace DeliveryApp.Application.DTOs.Response.Rental
{
    public class GetRentalResponseDTO
    {
        [JsonPropertyName("identificador")]
        public string Identifier { get; set; }

        [JsonPropertyName("valor_diaria")]
        public decimal DayliPrice { get; set; }

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

        [JsonPropertyName("data_devolucao")]
        public DateTime ReturnDate { get; set; }
    }
}
