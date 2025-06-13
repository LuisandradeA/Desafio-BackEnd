using DeliveryApp.Application.DTOs.Request.Rentals;
using DeliveryApp.Application.DTOs.Response;
using DeliveryApp.Application.DTOs.Response.Rental;

namespace DeliveryApp.Application.Services
{
    public interface IRentalService
    {
        Task<Response<DefaultResult>> CreateRentalAsync(CreateRentalDTO dto);
        Task<Response<GetRentalResponseDTO>> GetRentalByIdAsync(string id);
    }
}
