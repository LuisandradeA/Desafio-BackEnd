using DeliveryApp.Domain.Entities;

namespace DeliveryApp.Domain.Interfaces
{
    public interface IMotorcycleRepository
    {
        Task<bool> CreateAsync(Motorcycle motorcycle);
        Task<List<Motorcycle>> GetMotorcyclesAsync();
        Task<bool> UpdateMotorcycleAsync(Motorcycle motorcycle);
        Task<Motorcycle?> GetMotorcycleByIdAsync(string id);
    }
}
