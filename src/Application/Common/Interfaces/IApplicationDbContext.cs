
using Domain.Entities.Customers;
using Domain.Entities.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Customer> Customers { get; }
    DbSet<Transaction> Transactions { get; }
    DbSet<TransactionItem> TransactionItems { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
