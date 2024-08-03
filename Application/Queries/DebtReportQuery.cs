using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Data.Repositories;

namespace BookManagementSystem.Application.Queries
{
    public class DebtReportQuery : QueryObject
    {
        public int? Id { get; set; }
        public int? ReportMonth { get; set; }
        public int? ReportYear { get; set; }
    }
}
