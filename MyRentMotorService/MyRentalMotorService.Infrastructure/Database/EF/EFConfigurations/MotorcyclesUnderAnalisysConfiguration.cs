using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyRentMotorService.Domain.RentalAggregate.Entities;

namespace MyRentalMotorService.Infrastructure.Database.EF.EFConfigurations;

internal class MotorcyclesUnderAnalisysConfiguration : IEntityTypeConfiguration<MotorcyclesUnderAnalysis>
{
  public void Configure(EntityTypeBuilder<MotorcyclesUnderAnalysis> builder)
  {
    builder.HasKey(x => x.Id);

    builder.HasOne(r => r.Motorcycle)
      .WithOne()
      .HasForeignKey<MotorcyclesUnderAnalysis>("MotorcycleId")
      .OnDelete(DeleteBehavior.Cascade);
  }
}
