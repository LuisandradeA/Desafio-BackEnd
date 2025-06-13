using DeliveryApp.Domain.Entities;
using DeliveryApp.Domain.Interfaces;
using DeliveryApp.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.Infrastructure.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly DeliveryAppDbContext _context;

        public RentalRepository(DeliveryAppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(Rentals rental)
        {
            var entry = await _context.Rentals.AddAsync(rental);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Rentals?> GetRentalById(string id)
        {
            return await _context.Rentals
                .FirstOrDefaultAsync(d => d.Identifier == id);
        }
    }
}
