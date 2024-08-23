using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.DebtReportDetail;

namespace BookManagementSystem.Application.Dtos.DebtReport
{
    public class GetAllDebtReportDto
    {
        public required int Id { get; set; }
        public required int ReportMonth { get; set; }
        public required int ReportYear { get; set; }

        public List<AllDebtReportDetailDto> DebtReportDetails { get; set; }
    }
}
