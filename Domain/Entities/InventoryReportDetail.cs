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
        [Column(TypeName = "char(5)")]
        public required string ReportID { get; set; }
        [StringLength(5)]
        [Column(TypeName = "char(5)")]
        public required string BookID { get; set; }

        public required int InitalStock { get; set; }
        public required int FinalStock { get; set; }
        public required int AdditionalStock { get; set; }

        [ForeignKey("ReportID")]
        public virtual InventoryReport InventoryReport { get; set; } 

        [ForeignKey("BookID")]
        public virtual Book Book { get; set; }

    }
}


