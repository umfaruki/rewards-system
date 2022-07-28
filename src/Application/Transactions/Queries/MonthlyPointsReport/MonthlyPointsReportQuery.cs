using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Model;
using MediatR;

namespace Application.Transactions.Queries.MonthlyPointsReport;

public class MonthlyPointsReportQuery : IRequest<PaginatedList<MonthlyPointsReportDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}

public class MonthlyPointsReportQueryHandler : IRequestHandler<MonthlyPointsReportQuery, PaginatedList<MonthlyPointsReportDto>>
{
    private readonly IApplicationDbContext _context;

    public MonthlyPointsReportQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<PaginatedList<MonthlyPointsReportDto>> Handle(MonthlyPointsReportQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

