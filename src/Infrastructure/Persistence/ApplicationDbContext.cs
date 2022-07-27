using System.Reflection;
using Application.Common.Interfaces;
using Domain.Entities.Customers;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<Customer> Customers => Set<Customer>();

    public override  async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }
}
