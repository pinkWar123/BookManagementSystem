using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Dtos.InventoryReportDetail
{
    public class InventoryReportDetailDto
    {
        public required int ReportID { get; set; }
        public required int BookID { get; set; }
        public int InitialStock { get; set; }
        public int FinalStock { get; set; }
        public int AdditionalStock { get; set; }
    }
}