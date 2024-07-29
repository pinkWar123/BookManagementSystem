using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Dtos.InventoryReport
{
    public class CreateInventoryReportDto
    {
        public required int ReportMonth { get; set; }
        public required int ReportYear { get; set; }
    }
}