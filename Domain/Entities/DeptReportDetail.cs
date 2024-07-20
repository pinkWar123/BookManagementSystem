using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Domain.Entities
{
    public class DeptReportDetail{
        // properties: ReportID, CustomerID, InitialDept,  AdditionalDept
        public string? ReportID { get; set; }
        public string? CustomerID { get; set; }
        public double InitialDept { get; set; }
        public double AdditionalDept { get; set; }
    }
}