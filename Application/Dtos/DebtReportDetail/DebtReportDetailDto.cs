namespace BookManagementSystem.Application.Dtos.DebtReportDetail
{
    public class DebtReportDetailDto
    {
        public required string ReportID { get; set; }
        public required string CustomerID { get; set; }
        public int InitialDebt { get; set; }
        public int FinalDebt { get; set; }
        public int DebtChange { get; set; }
    }
}