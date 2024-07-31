namespace BookManagementSystem.Application.Dtos.DebtReportDetail
{
    public class DebtReportDetailDto
    {
        public required int ReportID { get; set; }
        public required int CustomerID { get; set; }
        public int InitialDebt { get; set; }
        public int FinalDebt { get; set; }
        public int AdditionalDebt { get; set; }
    }
}
