namespace BookManagementSystem.Application.Dtos.DebtReportDetail
{
    public class UpdateDebtReportDetailDto
    {
        public int? InitialDebt { get; set; }
        public int? FinalDebt { get; set; }
        public int? DebtChange { get; set; }
    }
}
