using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Dtos.InventoryReport
{
    public class UpdateInventoryReportDto
    {
        public int? ReportMonth { get; set; }
        public int? ReportYear { get; set; }
    }
}