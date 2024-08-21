using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyRentMotorService.Domain.RentalAggregate.Aggregate;

namespace MyRentalMotorService.Infrastructure.Database.EF.EFConfigurations;

internal class RentalConfiguration : IEntityTypeConfiguration<Rental>
{
    public void Configure(EntityTypeBuilder<Rental> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(r => r.Motorcycle)
          .WithMany()
          .HasForeignKey("MotorcycleId")
          .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Customer)
          .WithMany()
          .HasForeignKey("CustomerId")
          .OnDelete(DeleteBehavior.Restrict);
    }
}
