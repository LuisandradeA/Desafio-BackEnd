using DeliveryApp.Domain.Entities;

namespace DeliveryApp.Domain.Interfaces
{
    public interface IDriverRepository
    {
        Task<bool> CreateAsync(Driver motorcycle);
        Task<Driver?> GetByIdAsync(string id);
        Task<bool> UpdateAsync(Driver driver);
    }
}
