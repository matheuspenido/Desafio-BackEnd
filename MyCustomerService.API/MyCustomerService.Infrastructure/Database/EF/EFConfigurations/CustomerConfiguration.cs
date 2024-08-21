using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCustomerService.Domain.Entities;

namespace MyCustomerService.Infrastructure.Database.EF.EFConfigurations;

internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasAlternateKey(x => x.DriverLicense);
        builder.HasAlternateKey(x => x.Cnpj);
    }
}
