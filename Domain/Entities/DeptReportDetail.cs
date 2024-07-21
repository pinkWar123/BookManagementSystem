using System.ComponentModel.DataAnnotations.Schema;

namespace BookManagementSystem.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class DeptReportDetail
    {
        [StringLength(5)]
        [Column(TypeName = "char(5)")]
        public required string ReportID { get; set; }
        [StringLength(5)]
        [Column(TypeName = "char(5)")]
        public required string CustomerID { get; set; }

        public required int InitalDept { get; set; }
        public required int FinalDept { get; set; }
        public required int AdditionalDept { get; set; }

        [ForeignKey("ReportID")]
        public virtual DeptReport DeptReport { get; set; }

        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }

    }
}



