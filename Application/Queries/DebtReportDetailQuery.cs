using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Data.Repositories;

namespace BookManagementSystem.Application.Queries
{
    public class DebtReportDetailQuery : QueryObject
    {
        public int? ReportId { get; set; }
        public int? CustomerId { get; set; }
        public int? InitialDebt { get; set; }
        public int? FinalDebt { get; set; }
        public int? AdditionalDebt { get; set; }
    }
}
