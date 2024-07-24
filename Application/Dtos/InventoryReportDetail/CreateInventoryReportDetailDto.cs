using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BookManagementSystem.Application.Dtos.InventoryReportDetail
{
    public class CreateInventoryReportDetailDto
    {
        public required string ReportID { get; set; }
        public required string BookID { get; set; }
        public required int InitialStock { get; set; }
        public required int FinalStock { get; set; }
        public required int AdditionalStock { get; set; }
    }
}