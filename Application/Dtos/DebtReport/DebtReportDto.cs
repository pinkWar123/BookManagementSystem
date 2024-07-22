using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Dtos.DebtReport
{
    public class CreateDebtReportDto
    {
        public int? ReportMonth { get; set; }
        public int? ReportYear { get; set; }
    }

    public class UpdateDebtReportDto
    {
        public int? ReportMonth { get; set; }
        public int? ReportYear { get; set; }
    }

    public class DebtReportDto
    {
        public required string ReportID { get; set; }
        public required int ReportMonth { get; set; }
        public required int ReportYear { get; set; }
    }
}