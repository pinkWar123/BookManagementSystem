using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Dtos.DebtReport
{
    public class DebtReportDto
    {
        public required int Id { get; set; }
        public required int ReportMonth { get; set; }
        public required int ReportYear { get; set; }
    }
}
