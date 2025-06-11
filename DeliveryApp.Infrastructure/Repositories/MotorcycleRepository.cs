using DeliveryApp.Domain.Entities;
using DeliveryApp.Domain.Interfaces;
using DeliveryApp.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.Infrastructure.Repositories
{
    public class MotorcycleRepository : IMotorcycleRepository
    {
        private readonly DeliveryAppDbContext _context;
        
        public MotorcycleRepository(DeliveryAppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(Motorcycle motorcycle)
        {
            var entry = await _context.Motorcycles.AddAsync(motorcycle);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Motorcycle?> GetMotorcycleByIdAsync(string id)
        {
            return await _context.Motorcycles
                .Where(m => m.Identifier == id && !m.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Motorcycle>> GetMotorcyclesAsync()
        {
            return await _context.Motorcycles
                .Where(m => !m.IsDeleted)
                .ToListAsync();
        }

        public async Task<bool> UpdateMotorcycleAsync(Motorcycle motorcycle)
        {
            _context.Motorcycles.Update(motorcycle);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
