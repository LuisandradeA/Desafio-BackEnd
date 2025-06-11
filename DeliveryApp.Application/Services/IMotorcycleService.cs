using DeliveryApp.Application.DTOs;
using DeliveryApp.Application.DTOs.Request;
using DeliveryApp.Application.DTOs.Response;
using DeliveryApp.Application.DTOs.Response.Motorcycle;

namespace DeliveryApp.Application.Services
{
    public interface IMotorcycleService
    {
        Task<Response<DefaultResult>> CreateMotorcycleAsync(CreateMotorcycleDTO dto);
        Task<Response<List<GetMotorcyclesResponseDTO>>> GetMotorcyclesAsync();
        Task<Response<DefaultResult>> UpdateLicensePlateAsync(UpdateLicensePlateDTO dto);
        Task<Response<GetMotorcyclesResponseDTO>> GetMotorcycleByIdAsync(string id);
        Task<Response<DefaultResult>> DeleteMotorcycleAsync(string id);
    }
}
