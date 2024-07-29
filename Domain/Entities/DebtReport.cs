
namespace BookManagementSystem.Domain.Entities
{

    public class DebtReport : Base
    {
        public required int ReportMonth { get; set; }
        public required int ReportYear { get; set; }

        public ICollection<DebtReportDetail>? DebtReportDetails { get; set; }
    }
}



