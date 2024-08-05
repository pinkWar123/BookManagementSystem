using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Dtos.DebtReport;
using BookManagementSystem.Application.Dtos.DebtReportDetail;

public class MonthlyDebtUpdateService : IHostedService, IDisposable
{
    private Timer? _timer;
    private readonly IDebtReportService _debtReportService;
    private readonly IDebtReportDetailService _debtReportDetailService;
    private readonly ICustomerService _customerService;
    private readonly HttpClient _httpClient;

    public MonthlyDebtUpdateService(
        IDebtReportService debtReportService,
        IDebtReportDetailService debtReportDetailService,
        ICustomerService customerService,
        HttpClient httpClient
    )
    {
        _debtReportService = debtReportService;
        _debtReportDetailService = debtReportDetailService;
        _customerService = customerService;
        _httpClient = httpClient;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, GetTimeUntilNextRun(), TimeSpan.FromDays(28)); // Schedule to run every 28 days
        return Task.CompletedTask;
    }

    private async void DoWork(object? state)  // Mark state as nullable
    {
        await UpdateDebtReportDetailsForMonthAsync();
    }

    private async Task UpdateDebtReportDetailsForMonthAsync()
    {
        var now = DateTime.Now;
        int month = now.Month;
        int year = now.Year;

        // Create debt report
        var createDebtReportDto = new CreateDebtReportDto
        {
            ReportMonth = month,
            ReportYear = year
        };

        var content = new StringContent(JsonSerializer.Serialize(createDebtReportDto), System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("api/debt-report", content);

        if (response.IsSuccessStatusCode)
        {
            var responseData = await response.Content.ReadAsStringAsync();
            var createdDebtReport = JsonSerializer.Deserialize<DebtReportDto>(responseData);
            int reportId = createdDebtReport?.Id ?? 0;

            // Take previous debt report
            var previousMonth = now.AddMonths(-1);
            int previousMonth_Year = previousMonth.Year;
            int previousMonth_Month = previousMonth.Month;
            int previousReportID = await _debtReportService.GetReportIdByMonthYear(previousMonth_Year, previousMonth_Month);

            // Get all customer IDs
            var customerIds = await _customerService.GetAllCustomerId();

            // Loop through customer IDs and create debt report details
            foreach (var customerId in customerIds)
            {
                var previousDebtReportDetail = await _debtReportDetailService.GetDebtReportDetailById(previousReportID, customerId);

                var createDebtReportDetailDto = new CreateDebtReportDetailDto
                {
                    ReportID = reportId,
                    CustomerID = customerId,
                    InitialDebt = previousDebtReportDetail.FinalDebt,
                    FinalDebt = previousDebtReportDetail.FinalDebt
                };

                var detailContent = new StringContent(JsonSerializer.Serialize(createDebtReportDetailDto), System.Text.Encoding.UTF8, "application/json");
                var detailResponse = await _httpClient.PostAsync("api/debt-report-detail", detailContent);

                if (!detailResponse.IsSuccessStatusCode)
                {
                    // add exception later
                    throw new Exception("Failed to create debt report detail.");
                }
            }
        }
        else
        {
            // add exception later
            throw new Exception("Failed to create debt report.");
        }
    }

    private TimeSpan GetTimeUntilNextRun()
    {
        var now = DateTime.Now;
        var nextRunDate = new DateTime(now.Year, now.Month, 1).AddMonths(1);
        return nextRunDate - now;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
