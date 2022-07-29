using Application.Transactions.Queries.MonthlyPointsReport;
using Domain.Entities.Customers;
using Domain.Entities.Transactions;
using FluentAssertions;
using NUnit.Framework;

namespace Application.IntegrationTests.Transactions.Queries;

using static Testing;

public class MonthlyPointsReportTests : BaseTestFixture
{
   

    [Test]
    public async Task ShouldReturnMonthlyPointsReports()
    {
        
        await AddAsync(new Customer()
        {
             CustomerNumber = "11101",
             DateJoined = DateTime.UtcNow,
             Firstname = "John",
             Surname = "Doe",
             Email = "test@test.com",
             Telephone = "222020222",
            Transactions =
                    {
                        
                        new Transaction()
                        {
                            
                            MinSpentAmount = 50,
                            MinSpentAmountPoints = 1,
                            UpperRangeSpentAmount = 100,
                            UpperRangeSpentPoints = 1,
                            SubTotal = 120,
                            TotalVAT = 24,
                            GrandTotal = 144,
                            TransactionItems =
                            {
                                new TransactionItem()
                                {
                                    ItemDescription = "Apples",
                                    Quantity = 10,
                                    Price = 2
                                },
                                new TransactionItem()
                                {
                                    ItemDescription = "Crisps",
                                    Quantity = 20,
                                    Price = 4
                                },
                                new TransactionItem()
                                {
                                    ItemDescription = "Drinks",
                                    Quantity = 10,
                                    Price = 2
                                }
                            }

                        },
                        
                    }
        });

        var query = new MonthlyPointsReportQuery{};

        var result = await SendAsync(query);

        result.Items.Should().HaveCount(1);
        result.Items.First().MonthlyPoints.Should().HaveCountGreaterOrEqualTo(1);
    }

    
}
