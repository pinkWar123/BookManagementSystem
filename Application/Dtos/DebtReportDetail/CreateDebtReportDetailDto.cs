namespace BookManagementSystem.Application.Dtos.DebtReportDetail
{
    public class CreateDebtReportDetailDto
    {
        public string? ReportID { get; set; }
        public string? CustomerID { get; set; }
        public int? InitialDebt { get; set; }
        public int? FinalDebt { get; set; }
        public int? DebtChange { get; set; }
    }
}
