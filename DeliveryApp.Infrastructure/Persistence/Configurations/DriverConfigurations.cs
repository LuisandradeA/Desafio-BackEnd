using DeliveryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryApp.Infrastructure.Persistence.Configurations
{
    public class DriverConfigurations : IEntityTypeConfiguration<Driver>
    {
        public void Configure(EntityTypeBuilder<Driver> builder)
        {
            builder.ToTable("Drivers");

            builder.HasKey(m => m.Identifier);
            builder.Property(m => m.Identifier);

            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(m => m.Document)
                .IsUnique();

            builder.Property(m => m.Document)
                .HasMaxLength(14);

            builder.Property(m => m.BirthDate);

            builder.HasIndex(m => m.LicenseNumber)
                .IsUnique();

            builder.Property(m => m.LicenseNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(m => m.LicenseType);

            builder.Property(m => m.LicenseImagePath)
                .IsRequired(false)
                .HasMaxLength(255);

            builder.Property(m => m.CreatedTime); 
        }
    }
}
