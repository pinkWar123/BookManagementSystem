using System.ComponentModel.DataAnnotations.Schema;

namespace BookManagementSystem.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class DebtReportDetail
    {
        public int ReportID { get; set; }
        public int CustomerID { get; set; }

        public required int InitalDebt { get; set; }
        public required int FinalDebt { get; set; }
        public required int AdditionalDebt { get; set; }

        [ForeignKey("ReportID")]
        public virtual DebtReport DebtReport { get; set; }

        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }

    }
}



