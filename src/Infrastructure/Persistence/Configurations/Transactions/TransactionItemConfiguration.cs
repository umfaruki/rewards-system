using Domain.Entities.Customers;
using Domain.Entities.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Transactions;

public class TransactionItemConfiguration : IEntityTypeConfiguration<TransactionItem>
{
    public void Configure(EntityTypeBuilder<TransactionItem> builder)
    {
        builder.Property(t => t.ItemDescription)
            .IsRequired().HasMaxLength(100);

        builder.Property(t => t.Price)
           .IsRequired();
             
        builder.Property(t => t.Quantity)            
            .IsRequired();
            
        builder.Property(t => t.TransactionId)            
            .IsRequired();

        builder.HasOne<Transaction>(t => t.Transaction);
    }
}
