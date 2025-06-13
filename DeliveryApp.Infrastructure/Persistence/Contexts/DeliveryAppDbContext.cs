using DeliveryApp.Domain.Entities;
using DeliveryApp.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.Infrastructure.Persistence.Contexts
{
    public class DeliveryAppDbContext : DbContext
    {
        public DeliveryAppDbContext(DbContextOptions<DeliveryAppDbContext> options) : base(options)
        {
        }

        public DbSet<Motorcycle> Motorcycles { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Rentals> Rentals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new MotorcycleConfigurations());
            modelBuilder.ApplyConfiguration(new DriverConfigurations());
            modelBuilder.ApplyConfiguration(new RentalsConfigurations());
        }
    }
}
