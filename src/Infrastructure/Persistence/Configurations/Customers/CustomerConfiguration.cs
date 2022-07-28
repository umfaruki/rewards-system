using Domain.Entities.Customers;
using Domain.Entities.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Customers;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(t => t.CustomerNumber)
            .HasMaxLength(5)
            .IsRequired();

        builder.Property(t => t.DateJoined)           
           .IsRequired();

        builder.Property(t => t.Email)
          .HasMaxLength(150)
          .IsRequired();

        builder.Property(t => t.Firstname)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.Surname)
           .HasMaxLength(50)
           .IsRequired();

        builder.Property(t => t.Telephone)
            .HasMaxLength(15)
            .IsRequired();

        builder.HasMany<Transaction>(t => t.Transactions).WithOne(t => t.Customer);       

    }
}
