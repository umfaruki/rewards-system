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
        var query = new MonthlyPointsReportQuery
        {
            Months = new[] { 5 },
            Year = 2022
        };

        var result = await SendAsync(query);

        result.Should().HaveCount(1);
        result.First().MonthlyPoints.Should().HaveCountGreaterOrEqualTo(1);
    }

    
}
