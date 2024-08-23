using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Data.Repositories;

namespace BookManagementSystem.Application.Queries
{
    public class InventoryReportDetailQuery : QueryObject
    {
        public int? ReportID { get; set; }
        public int? BookID { get; set; }
        public int? InitialStock { get; set; }
        public int? FinalStock { get; set; }
        public int? AdditionalStock { get; set; }
    }
}