using Domain.Entities.Customers;
using Domain.Entities.Transactions;
using Infrastructure.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Infrastructure.Persistence;

public class ApplicationDbContextInitializer
{
    private readonly ILogger<ApplicationDbContextInitializer> _logger;
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextInitializer(ILogger<ApplicationDbContextInitializer> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsNpgsql())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {            

        

        // Default data
        // Seed, if necessary
        if (!_context.Customers.Any())
        {
            if (!_context.RewardSettings.Any())
            {
                _context.RewardSettings.Add(new Domain.Entities.RewardSettings.RewardSetting 
                {
                    MinSpentAmount = 50,
                    MinSpentAmountPoints = 1,
                    UpperRangeSpentAmount = 100,
                    UpperRangeSpentPoints = 1,
                    TaxRate = 20
                });

                _context.SaveChanges();
            }

            var rewardSettings = _context.RewardSettings.FirstOrDefault() ?? new Domain.Entities.RewardSettings.RewardSetting();

            // Loading Mock data from json files
            var customerList = MockDataHelper.GetMockData<Customer>();
            var transactionsList = MockDataHelper.GetMockData<Transaction>();
            var transactionsItemsList = MockDataHelper.GetMockData<TransactionItem>();
            var rnd = new Random();

            customerList.ForEach(customer =>
            {
                customer.Transactions = new List<Transaction>();
                var currentCustomerTransactions = transactionsList.Skip((customer.Id - 1) * 10).Take(10).ToList();

                var i = 0;
                
                currentCustomerTransactions.ForEach(transaction => 
                {
                    var randNumber = rnd.Next(1, 10);

                    transaction.TaxRate = rewardSettings.TaxRate;
                    transaction.CustomerId = customer.Id;
                    transaction.Customer = customer;
                    transaction.MinSpentAmount = rewardSettings.MinSpentAmount;
                    transaction.MinSpentAmountPoints = rewardSettings.MinSpentAmountPoints;
                    transaction.UpperRangeSpentAmount = rewardSettings.UpperRangeSpentAmount; 
                    transaction.UpperRangeSpentPoints = rewardSettings.UpperRangeSpentPoints;                    
                    
                    transaction.TransactionItems = new List<TransactionItem>();
                   
                    var currentTransactionItems = transactionsItemsList.Take(randNumber).ToList();
                    
                    currentTransactionItems.ForEach(transactionItem => 
                    {
                        transactionItem.Transaction = transaction;                        
                        transaction.TransactionItems.Add(transactionItem);
                    });

                    transaction.SubTotal = currentTransactionItems.Sum(x => (x.Price * x.Quantity));
                    transaction.TotalVAT = transaction.SubTotal * (transaction.TaxRate / 100);
                    transaction.GrandTotal = transaction.SubTotal + transaction.SubTotal;
                    customer.Transactions.Add(transaction);

                    transactionsItemsList.RemoveAll(x => currentTransactionItems.Contains(x));
                    i += 10;
                });
               
            });

            _context.SaveChanges();

        }
    }


}
