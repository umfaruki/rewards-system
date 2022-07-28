using Application.Common.Model;
using Application.Transactions.Queries.MonthlyPointsReport;
using Microsoft.AspNetCore.Mvc;

namespace Rewards.Api.Controllers
{
    public class TransactionController : ApiControllerBase 
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedList<MonthlyPointsReportDto>>> MonthlyPointsReports([FromQuery] MonthlyPointsReportQuery query)
        {
            return await Mediator.Send(query);
        }
    }
}
