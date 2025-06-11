using System.Text.Json.Serialization;

namespace DeliveryApp.Application.DTOs.Response
{
    public class Response<TResult>
    {
        public Response(bool success, string error = "")
        {
            Success = success;
            Message = error;
        }
        public bool Success { get; }
        public string Message { get; }
        [JsonIgnore]
        public bool IsServerError { get; set; }
        [JsonIgnore]
        public bool IsBadRequest { get; set; }
        public TResult? Result { get; set; }
    }
}
