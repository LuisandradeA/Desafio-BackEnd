using DeliveryApp.Domain.Entities;
using DeliveryApp.Domain.Interfaces;
using DeliveryApp.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.Infrastructure.Repositories
{
    public class DriverRepository : IDriverRepository
    {
        private readonly DeliveryAppDbContext _context;

        public DriverRepository(DeliveryAppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(Driver driver)
        {
            var entry = await _context.Drivers.AddAsync(driver);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Driver?> GetByIdAsync(string id)
        {
            return await _context.Drivers
                .FirstOrDefaultAsync(d => d.Identifier == id);
        }

        public async Task<bool> UpdateAsync(Driver driver)
        {
            _context.Drivers.Update(driver);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
