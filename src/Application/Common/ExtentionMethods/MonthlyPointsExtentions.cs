using Application.Common.Model;
using Application.Transactions.Queries.MonthlyPointsReport;

namespace Application.Common.ExtentionMethods;

public static class MonthlyPointsExtentions
{
    public static List<MonthlyPointsReportDto> GenerateReport(this List<RawTransactionsData> transactionData, MonthlyPointsReportQuery request)
    {
        var monthlyPointsReports = new List<MonthlyPointsReportDto>();

        var uniqueCustomers = transactionData.Select(x => new { x.CustomerId, x.Firstname, x.Surname, x.CustomerNumber }).Distinct().ToList();

        uniqueCustomers.ForEach(customer =>
        {
            var customerReport = new MonthlyPointsReportDto();
            customerReport.FirstName = customer.Firstname;
            customerReport.Surname = customer.Surname;
            customerReport.CustomerNumber = customer.CustomerNumber;

            request.Months.ToList().ForEach(month =>
            {
                var customerMonthlyData = transactionData.FirstOrDefault(x => x.Year == request.Year && x.Month == month && x.CustomerId == customer.CustomerId);
                if (customerMonthlyData != null)
                {
                    customerReport.MonthlyPoints.Add(new MonthlyPointsReportVm
                    {
                        CustomerId = customer.CustomerId,
                        Month = month,
                        MonthAndYear = $"{month}/{request.Year.ToString()}",
                        Year = request.Year,
                        MonthlyTotal = customerMonthlyData.TotalPoints,

                    });
                }
            });

            customerReport.TotalPoints = customerReport.MonthlyPoints.Sum(x => x.MonthlyTotal);

            monthlyPointsReports.Add(customerReport);
        });

        return monthlyPointsReports;
    }
}
