using DeliveryApp.Application.DTOs.Request.Driver;
using DeliveryApp.Application.DTOs.Response;
using Microsoft.AspNetCore.Http;

namespace DeliveryApp.Application.Services
{
    public interface IDriverService
    {
        Task<Response<DefaultResult>> CreateDriverAsync(CreateDriverDTO dto);
        Task<Response<DefaultResult>> UpdateLicensePlateImageAsync(string id, string base64String);
    }
}
