using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Application.Transactions.Queries.MonthlyPointsReport;

public class MonthlyPointsReportQuery : IRequest<PaginatedList<MonthlyPointsReportDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public int  Year { get; set; }
    public int[] Months { get; set; }
}

public class MonthlyPointsReportQueryHandler : IRequestHandler<MonthlyPointsReportQuery, PaginatedList<MonthlyPointsReportDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IDapperMonthlyReportHandler _dapperTransactionReportRepository;


    public MonthlyPointsReportQueryHandler(IApplicationDbContext context, 
        IDapperMonthlyReportHandler dapperTransactionReportRepository)
    {
        _context = context;
        _dapperTransactionReportRepository = dapperTransactionReportRepository;
    }

    public async Task<PaginatedList<MonthlyPointsReportDto>> Handle(MonthlyPointsReportQuery request, CancellationToken cancellationToken)
    {



        await _dapperTransactionReportRepository.CalculateTransactionPoints(request.Year, request.Months);

/*
        var transactions = await _context
            .Transactions.GroupBy(t => new
            {
                Month = t.DateCreated.Month,
                Year = t.DateCreated.Year,
                CustomerId = t.CustomerId,
                
            }).Select(tt => new MonthlyPointsReportVm()
            {
                CustomerId = tt.Key.CustomerId,
                Month = tt.Key.Month,
                Year = tt.Key.Year
            }).ToListAsync();

        */
        throw new NotImplementedException();


    }
}

