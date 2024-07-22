using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Dtos.DebtReport
{
    public class UpdateDebtReportDto
    {
        public int? ReportMonth { get; set; }
        public int? ReportYear { get; set; }
    }
}