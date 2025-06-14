using DeliveryApp.Domain.Entities;

namespace DeliveryApp.Domain.Interfaces
{
    public interface IRentalRepository
    {
        Task<bool> CreateAsync(Rentals rental);
        Task<Rentals?> GetRentalByIdAsync(string id);
        Task<bool> UpdateAsync(Rentals rental);
    }
}
