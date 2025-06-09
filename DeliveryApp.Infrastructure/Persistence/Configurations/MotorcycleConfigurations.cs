using DeliveryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryApp.Infrastructure.Persistence.Configurations
{
    public class MotorcycleConfigurations : IEntityTypeConfiguration<Motorcycle>
    {
        public void Configure(EntityTypeBuilder<Motorcycle> builder)
        {
            builder.ToTable("Motorcycles");
            
            builder.HasKey(m => m.Identifier);
            builder.Property(m => m.Identifier)
                .ValueGeneratedOnAdd();

            builder.Property(m => m.ModelName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.Year)
                .IsRequired();

            builder.HasIndex(m => m.LicensePlate)
                .IsUnique();

            builder.Property(m => m.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}
