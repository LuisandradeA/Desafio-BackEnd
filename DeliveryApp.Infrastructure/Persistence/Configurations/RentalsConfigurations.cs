using DeliveryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryApp.Infrastructure.Persistence.Configurations
{
    public class RentalsConfigurations : IEntityTypeConfiguration<Rentals>
    {
        public void Configure(EntityTypeBuilder<Rentals> builder)
        {
            builder.ToTable("Rentals");

            builder.HasKey(m => m.Identifier);
            builder.Property(m => m.Identifier);

            builder.Property(r => r.PlanType)
                .IsRequired();

            builder.Property(r => r.StartTime)
                .IsRequired();

            builder.Property(r => r.EndTime)
                .IsRequired();

            builder.Property(r => r.PrevisionTime)
                .IsRequired();

            builder.Property(r => r.MotorcycleId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(r => r.DriverId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(r => r.CreatedTime)
                .IsRequired();

            builder.HasIndex(r => r.MotorcycleId)
                .IsUnique();

            builder.HasIndex(r => r.DriverId)
                .IsUnique();

            builder.Property(r => r.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(r => r.AdditionalFee)
                .HasColumnType("decimal(18,2)")
                .IsRequired(false);

            builder.Property(r => r.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.HasOne(r => r.Motorcycle)
               .WithMany(m => m.Rentals)
               .HasForeignKey(r => r.MotorcycleId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Driver)
                .WithMany(d => d.Rentals)
                .HasForeignKey(r => r.DriverId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
