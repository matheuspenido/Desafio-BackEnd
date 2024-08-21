using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyMotorcycleService.Domain.Entities;

namespace MyMotorcycleService.Infrastructure.Database.EF.EFConfigurations;

internal class MotorcycleConfiguration : IEntityTypeConfiguration<Motorcycle>
{
    public void Configure(EntityTypeBuilder<Motorcycle> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasAlternateKey(x => x.LicensePlate);
    }
}
