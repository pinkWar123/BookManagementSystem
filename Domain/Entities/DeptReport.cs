using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class DeptReport : Base
    {
        public required int ReportMonth { get; set; }
        public required int ReportYear { get; set; }

        public ICollection<DeptReportDetail>? DeptReportDetails { get; set; }
    }
}



