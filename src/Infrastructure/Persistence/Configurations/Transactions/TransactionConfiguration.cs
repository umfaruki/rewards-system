using Domain.Entities.Customers;
using Domain.Entities.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Transactions;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.Property(t => t.DateCreated)
            .IsRequired();

        builder.Property(t => t.MinSpentAmount)
           .IsRequired();
             
        builder.Property(t => t.UpperRangeSpentAmount)            
            .IsRequired();
            
        builder.Property(t => t.CustomerId)            
            .IsRequired();

        builder.HasOne<Customer>(t => t.Customer);
    }
}
