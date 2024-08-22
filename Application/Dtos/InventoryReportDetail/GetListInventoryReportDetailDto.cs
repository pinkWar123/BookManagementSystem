using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Dtos.InventoryReportDetail
{
    public class GetListInventoryReportDetailDto
    {
        public required int ReportID { get; set; }
        public required int BookID { get; set; }
        public required string Title { get; set; }  // Đổi từ `title` thành `Title`
        public int InitialStock { get; set; }
        public int FinalStock { get; set; }
        public int AdditionalStock { get; set; }
    }

}