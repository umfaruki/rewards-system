using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.ExtentionMethods;
using Application.Common.Interfaces;
using Application.Common.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Application.Transactions.Queries.MonthlyPointsReport;

public class MonthlyPointsReportQuery : IRequest<List<MonthlyPointsReportDto>>
{    
    public int  Year { get; set; }
    public int[] Months { get; set; }
}

public class MonthlyPointsReportQueryHandler : IRequestHandler<MonthlyPointsReportQuery, List<MonthlyPointsReportDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IDapperMonthlyReportHandler _dapperTransactionReportRepository;


    public MonthlyPointsReportQueryHandler(IApplicationDbContext context, 
        IDapperMonthlyReportHandler dapperTransactionReportRepository)
    {
        _context = context;
        _dapperTransactionReportRepository = dapperTransactionReportRepository;
    }

    public async Task<List<MonthlyPointsReportDto>> Handle(MonthlyPointsReportQuery request, CancellationToken cancellationToken)
    {
        await _dapperTransactionReportRepository.CalculateTransactionPoints(request.Year, request.Months);
        var transactionData = await _dapperTransactionReportRepository.GetTransactionData(request.Year, request.Months);

        var monthlyPointsReports = transactionData.GenerateReport(request);

        return monthlyPointsReports;
    }
}

