using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Transactions.Queries.MonthlyPointsReport
{
    public class MonthlyPointsReportVm
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int CustomerId { get; set; }
        public string MonthAndYear { get; set; }
        public Decimal MonthlyTotal { get; set; }
    }
}
