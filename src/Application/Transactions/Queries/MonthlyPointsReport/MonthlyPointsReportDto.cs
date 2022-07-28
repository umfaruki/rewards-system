using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Transactions.Queries.MonthlyPointsReport
{
    public class MonthlyPointsReportDto
    {
        public string CustomerNumber { get; set; }
        public string FirstName { get; set; }   
        public string Surname { get; set; } 

        public List<MonthlyPointsReportVm> MonthlyPoints { get; set; }
        public decimal TotalPoints { get; set; }

    }
}
