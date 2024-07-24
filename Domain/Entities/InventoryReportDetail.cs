

namespace BookManagementSystem.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class InventoryReportDetail
    {
        public int ReportID { get; set; }
        public int BookID { get; set; }

        public required int InitalStock { get; set; }
        public required int FinalStock { get; set; }
        public required int AdditionalStock { get; set; }

        [ForeignKey("ReportID")]
        public virtual InventoryReport InventoryReport { get; set; }

        [ForeignKey("BookID")]
        public virtual Book Book { get; set; }

    }
}


