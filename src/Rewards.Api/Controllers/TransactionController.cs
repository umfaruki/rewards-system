using Application.Common.Model;
using Application.Transactions.Queries.MonthlyPointsReport;
using Microsoft.AspNetCore.Mvc;

namespace Rewards.Api.Controllers
{
    public class TransactionController : ApiControllerBase 
    {
        [HttpGet]
        [Route("monthly-points-reports/{year}/{months}")]
        public async Task<ActionResult<List<MonthlyPointsReportDto>>> MonthlyPointsReports(int year, string months)
        {
            var query = new MonthlyPointsReportQuery();
            query.Year = year;

            // Just to make sure user input is a number
            if (months.Replace(',', '0').All(char.IsDigit))
                query.Months = (months ?? "").Split(',').Select(int.Parse).ToArray();
            return await Mediator.Send(query);
        }
    }
}
