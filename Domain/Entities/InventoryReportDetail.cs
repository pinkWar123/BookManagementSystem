using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class InventoryReportDetail
    {
        [StringLength(5)]
        public required string ReportID { get; set; }
        [StringLength(5)]
        public required string BookID { get; set; }

        public required int initalStock { get; set; }
        public required int finalStock { get; set; }
        public required int additionalStock { get; set; }

        [ForeignKey("ReportID")]
        public virtual InventoryReport InventoryReport { get; set; } 

        [ForeignKey("BookID")]
        public virtual Book Book { get; set; }

    }
}


