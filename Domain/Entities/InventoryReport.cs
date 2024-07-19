using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class InventoryReport : Base
    {
        public required int reportMonth { get; set; }
        public required int reportYear { get; set; }

        public ICollection<InventoryReportDetail>? InventoryReportDetails { get; set; }
    }
}

