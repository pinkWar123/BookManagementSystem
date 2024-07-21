
namespace BookManagementSystem.Domain.Entities
{

    public class DeptReport : Base
    {
        public required int ReportMonth { get; set; }
        public required int ReportYear { get; set; }

        public ICollection<DeptReportDetail>? DeptReportDetails { get; set; }
    }
}



