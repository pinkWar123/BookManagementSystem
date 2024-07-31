namespace BookManagementSystem.Application.Dtos.DebtReportDetail
{
    public class CreateDebtReportDetailDto
    {
        public int? ReportID { get; set; }
        public int? CustomerID { get; set; }
        public int? InitialDebt { get; set; }
        public int? FinalDebt { get; set; }
        public int? AdditionalDebt { get; set; }
    }
}
