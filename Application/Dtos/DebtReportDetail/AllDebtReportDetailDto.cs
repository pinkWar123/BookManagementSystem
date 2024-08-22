namespace BookManagementSystem.Application.Dtos.DebtReportDetail
{
    public class AllDebtReportDetailDto
    {
        public int ReportID { get; set; }
        public int CustomerID { get; set; }
        public required string customerName { get; set; }
        public int InitialDebt { get; set; }
        public int FinalDebt { get; set; }
        public int AdditionalDebt { get; set; }
    }
}
