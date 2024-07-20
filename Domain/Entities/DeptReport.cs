using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Domain.Entities
{
    public class DeptReport{
        // properties: reportID, reportMonth, reportYear
        public string? ReportID { get; set; }
        public int ReportMonth { get; set; }
        public int ReportYear { get; set; }
        
    }
}