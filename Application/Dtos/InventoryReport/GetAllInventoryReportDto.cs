using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.InventoryReportDetail;

namespace BookManagementSystem.Application.Dtos.InventoryReport
{
    public class GetAllInventoryReportDto 
    {
        public required int ReportID { get; set; }
        public required int ReportMonth { get; set; }
        public required int ReportYear { get; set; }

        public List<GetListInventoryReportDetailDto> InventoryReportDetails { get; set; }
    }
}